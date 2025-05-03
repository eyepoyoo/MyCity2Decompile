using System;
using System.Collections.Generic;
using DeepThought;
using UnityEngine;

public class StatNeuron : Neuron, IOverridable
{
	private EStatisticsRule _rule;

	private bool _dirty;

	private string _initialValue;

	protected Neuron _type;

	public StatNeuron(Neuron parent, CreationParameters parameters)
		: base(parent, parameters)
	{
		Initialise(tryGetChild("rule"));
	}

	public void Reset()
	{
		base.Text = _initialValue;
		_dirty = false;
	}

	protected virtual void SetValue(float value)
	{
		base.Number = value;
	}

	protected virtual void SetValue(int value)
	{
		base.Number = value;
	}

	protected virtual void SetValue(bool value)
	{
		base.Boolean = value;
	}

	protected virtual void SetValue(string value)
	{
		base.Text = value;
	}

	public virtual float GetSafeFloat()
	{
		return base.Number;
	}

	public virtual int GetSafeInt()
	{
		return (int)base.Number;
	}

	public virtual bool GetSafeBool()
	{
		return base.Boolean;
	}

	public virtual string GetSafeString()
	{
		return base.Text;
	}

	protected virtual void SetCSVValue(Neuron node)
	{
		this.SetStringValues(node.GetStringValues());
	}

	protected virtual void SetDateTimeValue(DateTime value)
	{
		this.SetDateTime(value);
	}

	public void OverrideStat(float value)
	{
		SetValue(value);
		_dirty = true;
	}

	public void OverrideStat(int value)
	{
		SetValue(value);
		_dirty = true;
	}

	public void OverrideStat(bool value)
	{
		SetValue(value);
		_dirty = true;
	}

	public void OverrideStat(string value)
	{
		SetValue(value);
		_dirty = true;
	}

	public void OverrideCSVStat(Neuron node)
	{
		SetCSVValue(node);
		_dirty = true;
	}

	public void ModifyStat(float value, EStatisticsRule rule = EStatisticsRule.NO_RULE)
	{
		switch (ChooseRule(rule))
		{
		case EStatisticsRule.INCREMENT:
			SetValue(GetSafeFloat() + value);
			break;
		case EStatisticsRule.KEEP_HIGHEST:
			SetValue(Mathf.Max(GetSafeFloat(), value));
			break;
		case EStatisticsRule.KEEP_LOWEST:
			SetValue(Mathf.Min(GetSafeFloat(), value));
			break;
		case EStatisticsRule.MULTIPLY:
			SetValue(GetSafeFloat() * value);
			break;
		case EStatisticsRule.KEEP_IF_DIRTY:
			if (!_dirty)
			{
				SetValue(value);
				Debug.Log("Value is clean, setting to " + value);
				_dirty = false;
			}
			else
			{
				Debug.Log("Value is dirty, preserving");
			}
			break;
		case EStatisticsRule.OVERRIDE_ALWAYS:
			SetValue(value);
			_dirty = false;
			break;
		}
	}

	public void ModifyDateTimeStat(Neuron node, EStatisticsRule rule = EStatisticsRule.NO_RULE)
	{
		DateTime dateTime = node.GetDateTime();
		DateTime dateTime2 = this.GetDateTime();
		switch (ChooseRule(rule))
		{
		case EStatisticsRule.INCREMENT:
		case EStatisticsRule.MULTIPLY:
			Debug.LogWarning("Cannot modify date time value of stat '" + getName() + "' with rule '" + _rule.ToString() + "'");
			break;
		case EStatisticsRule.KEEP_HIGHEST:
			if (DateTime.Compare(dateTime2, dateTime) <= 0)
			{
				SetDateTimeValue(dateTime);
			}
			else
			{
				SetDateTimeValue(dateTime2);
			}
			break;
		case EStatisticsRule.KEEP_LOWEST:
			if (DateTime.Compare(dateTime2, dateTime) > 0)
			{
				SetDateTimeValue(dateTime);
			}
			else
			{
				SetDateTimeValue(dateTime2);
			}
			break;
		case EStatisticsRule.KEEP_IF_DIRTY:
			if (!_dirty)
			{
				SetDateTimeValue(dateTime);
				Debug.Log("Value is clean, setting to " + base.Number);
				_dirty = false;
			}
			else
			{
				Debug.Log("Value is dirty, preserving");
			}
			break;
		case EStatisticsRule.OVERRIDE_ALWAYS:
			SetDateTimeValue(dateTime);
			_dirty = false;
			break;
		}
	}

	public void ModifyStat(bool value, EStatisticsRule rule = EStatisticsRule.NO_RULE)
	{
		switch (ChooseRule(rule))
		{
		case EStatisticsRule.INCREMENT:
		case EStatisticsRule.KEEP_HIGHEST:
		case EStatisticsRule.KEEP_LOWEST:
		case EStatisticsRule.MULTIPLY:
			Debug.LogWarning("Cannot modify boolean value of stat '" + getName() + "' with rule 'MULTIPLY'");
			break;
		case EStatisticsRule.KEEP_IF_DIRTY:
			if (!_dirty)
			{
				SetValue(value);
				Debug.Log("Value is clean, setting to " + base.Value.Bool);
				_dirty = false;
			}
			else
			{
				Debug.Log("Value is dirty, preserving");
			}
			break;
		case EStatisticsRule.OVERRIDE_ALWAYS:
			SetValue(value);
			_dirty = false;
			break;
		}
	}

	public void ModifyStat(string value, EStatisticsRule rule = EStatisticsRule.NO_RULE)
	{
		switch (ChooseRule(rule))
		{
		case EStatisticsRule.INCREMENT:
			SetValue(GetSafeString() + value);
			break;
		case EStatisticsRule.KEEP_HIGHEST:
			if (value.CompareTo(base.Text) > 0)
			{
				SetValue(value);
			}
			break;
		case EStatisticsRule.KEEP_LOWEST:
			if (value.CompareTo(base.Text) < 0)
			{
				SetValue(value);
			}
			break;
		case EStatisticsRule.MULTIPLY:
			break;
		case EStatisticsRule.KEEP_IF_DIRTY:
			if (!_dirty)
			{
				SetValue(value);
				_dirty = false;
			}
			break;
		case EStatisticsRule.OVERRIDE_ALWAYS:
			SetValue(value);
			_dirty = false;
			break;
		}
	}

	public void ModifyCSVStat(Neuron root, EStatisticsRule rule = EStatisticsRule.NO_RULE)
	{
		Debug.Log("Modifying CSV stat " + getName() + " with " + ChooseRule(rule));
		switch (ChooseRule(rule))
		{
		case EStatisticsRule.INCREMENT:
		{
			int capacity = base.numChildren + root.numChildren;
			List<string> list = new List<string>(capacity);
			Extensions.AddStringValuesToList(this, list);
			Extensions.AddStringValuesToList(root, list);
			list.Sort();
			for (int num = list.Count - 1; num > 0; num--)
			{
				Debug.Log("index = " + num);
				if (list[num] == list[num - 1])
				{
					list.RemoveAt(num);
				}
			}
			this.SetStringValues(list.ToArray());
			break;
		}
		case EStatisticsRule.KEEP_IF_DIRTY:
			if (!_dirty)
			{
				SetCSVValue(root);
				Debug.Log("Value is clean, setting to " + base.Text);
				_dirty = false;
			}
			else
			{
				Debug.Log("Value is dirty, preserving");
			}
			break;
		case EStatisticsRule.OVERRIDE_ALWAYS:
			SetCSVValue(root);
			_dirty = false;
			break;
		case EStatisticsRule.KEEP_HIGHEST:
		case EStatisticsRule.KEEP_LOWEST:
		case EStatisticsRule.MULTIPLY:
			Debug.LogWarning("Cannot modify string value of stat '" + getName() + "' with rule 'KEEP_HIGHEST'");
			break;
		}
	}

	private EStatisticsRule ChooseRule(EStatisticsRule rule)
	{
		if (rule == EStatisticsRule.NO_RULE)
		{
			return _rule;
		}
		return rule;
	}

	private void Initialise(Neuron rule)
	{
		_initialValue = base.Text;
		_type = this.VerifyProperty("type", "number");
		if (rule != null)
		{
			switch (rule.Text)
			{
			case "KeepHigher":
				_rule = EStatisticsRule.KEEP_HIGHEST;
				break;
			case "KeepLower":
				_rule = EStatisticsRule.KEEP_LOWEST;
				break;
			case "Multiply":
				_rule = EStatisticsRule.MULTIPLY;
				break;
			case "Dirty":
				_rule = EStatisticsRule.KEEP_IF_DIRTY;
				break;
			case "Increment":
				_rule = EStatisticsRule.INCREMENT;
				break;
			default:
				_rule = EStatisticsRule.OVERRIDE_ALWAYS;
				break;
			}
		}
	}

	public void Override(Neuron node)
	{
		switch (_type.Text)
		{
		case "bool":
			ModifyStat(node.Value.Bool, EStatisticsRule.OVERRIDE_ALWAYS);
			break;
		default:
			ModifyStat(node.Value.Float, EStatisticsRule.OVERRIDE_ALWAYS);
			break;
		case "text":
			ModifyStat(node.Value.String, EStatisticsRule.OVERRIDE_ALWAYS);
			break;
		case "csv":
			ModifyCSVStat(node, EStatisticsRule.OVERRIDE_ALWAYS);
			break;
		case "datetime":
			ModifyDateTimeStat(node, EStatisticsRule.OVERRIDE_ALWAYS);
			break;
		}
	}
}
