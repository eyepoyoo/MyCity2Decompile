using System;
using System.Collections.Generic;

namespace DeepThought
{
	public class Value
	{
		private float number;

		private string text;

		private bool boolean;

		private Neuron neuron;

		private byte type;

		private Neuron parent;

		private List<Neuron> observers;

		public string String
		{
			get
			{
				return getText();
			}
			set
			{
				setText(value);
			}
		}

		public bool Bool
		{
			get
			{
				return getBoolean();
			}
			set
			{
				setBoolean(value);
			}
		}

		public int Int
		{
			get
			{
				return (int)getNumber();
			}
			set
			{
				setNumber(value);
			}
		}

		public float Float
		{
			get
			{
				return getNumber();
			}
			set
			{
				setNumber(value);
			}
		}

		public Neuron NeuronRef
		{
			get
			{
				return getNeuron();
			}
			set
			{
				setNeuron(value);
			}
		}

		public Value(Neuron parent, string xml)
		{
			this.parent = parent;
			text = xml;
			type = ValueType.TEXT;
		}

		public static void getTypes(List<string> types)
		{
			types.Add("Number");
			types.Add("Text");
			types.Add("Boolean");
			types.Add("Neuron");
		}

		public void setParent(Neuron parent)
		{
			this.parent = parent;
		}

		private void change()
		{
			if (observers != null)
			{
				for (int i = 0; i < observers.Count; i++)
				{
					Neuron neuron = observers[i];
					if (!neuron.Alive)
					{
						observers.RemoveAt(i);
						i--;
						continue;
					}
					Neuron neuron2 = neuron.tryGetChild("valueChanged");
					if (neuron2 != null)
					{
						neuron2.setName("dumbChild");
						neuron2.Dispose();
					}
					neuron.message("<Neuron name=\"valueChanged\" valueName=\"" + parent.getName() + "\" reference=\"" + parent.getPath() + "\" />");
				}
			}
			if (parent != null)
			{
				parent.change(parent.getPath() + "." + getType());
			}
			parentChangeOnType();
		}

		public void parentChangeOnType()
		{
			if (parent != null)
			{
				if (type == ValueType.NUMBER)
				{
					parent.change(ChangeType.NEURON_NUMBER_CHANGED, parent);
				}
				else if (type == ValueType.TEXT)
				{
					parent.change(ChangeType.NEURON_TEXT_CHANGED, parent);
				}
				else if (type == ValueType.BOOLEAN)
				{
					parent.change(ChangeType.NEURON_BOOLEAN_CHANGED, parent);
				}
				else if (type == ValueType.NEURON)
				{
					parent.change(ChangeType.NEURON_NEURON_CHANGED, parent);
				}
			}
		}

		public void addObserver(Neuron neuron)
		{
			if (observers == null)
			{
				observers = new List<Neuron>();
			}
			observers.Add(neuron);
		}

		public void removeObserver(Neuron neuron)
		{
			observers.Remove(neuron);
		}

		public void update()
		{
			change();
		}

		public void copy(Value rhs)
		{
			if (type == ValueType.TEXT)
			{
				setText(rhs.getText());
			}
			else if (type == ValueType.BOOLEAN)
			{
				setBoolean(rhs.getBoolean());
			}
			else if (type == ValueType.NEURON)
			{
				setNeuron(rhs.getNeuron());
			}
			else if (type == ValueType.NUMBER)
			{
				setNumber(rhs.getNumber());
			}
		}

		public void copyWithType(Value copyFrom)
		{
			switch (copyFrom.getType())
			{
			case "Neuron":
				setNeuron(copyFrom.getNeuron());
				break;
			case "Number":
				setNumber(copyFrom.getNumber());
				break;
			case "Boolean":
				setBoolean(copyFrom.getBoolean());
				break;
			case "Text":
				setText(copyFrom.getText());
				break;
			default:
				throw new Exception("Invalid type");
			}
		}

		private Neuron textToNeuron(string xml)
		{
			Neuron neuron = null;
			neuron = parent.tryGetNeuron(xml);
			if (neuron != null)
			{
				return neuron;
			}
			neuron = parent.getRoot().tryGetNeuron(xml);
			if (neuron != null)
			{
				return neuron;
			}
			return neuron;
		}

		public void setNumber(float number, bool noChange)
		{
			if (type == ValueType.NUMBER)
			{
				if (number == this.number)
				{
					return;
				}
				this.number = number;
			}
			else
			{
				type = ValueType.NUMBER;
				this.number = number;
			}
			if (!noChange)
			{
				change();
			}
		}

		public void setNumber(float number)
		{
			setNumber(number, false);
		}

		public void setTextInitial(string text)
		{
			setText(text, true);
		}

		public void setText(string text)
		{
			setText(text, false);
		}

		public void setText(string text, bool noChange)
		{
			if (type == ValueType.TEXT)
			{
				if (text == this.text)
				{
					return;
				}
				this.text = text;
			}
			else
			{
				type = ValueType.TEXT;
				this.text = text;
			}
			if (!noChange)
			{
				change();
			}
		}

		public string getText()
		{
			if (type == ValueType.TEXT)
			{
				return text;
			}
			if (type == ValueType.BOOLEAN)
			{
				return string.Empty + boolean;
			}
			if (type == ValueType.NUMBER)
			{
				return string.Empty + number;
			}
			if (type == ValueType.NEURON)
			{
				return neuron.getPath();
			}
			throw new Exception("Invalid type");
		}

		public void setBoolean(bool boolean)
		{
			setBoolean(boolean, false);
		}

		public void setBoolean(bool boolean, bool noChange)
		{
			if (type == ValueType.BOOLEAN)
			{
				if (boolean == this.boolean)
				{
					return;
				}
				this.boolean = boolean;
			}
			else
			{
				type = ValueType.BOOLEAN;
				this.boolean = boolean;
			}
			if (!noChange)
			{
				change();
			}
		}

		public bool getBoolean()
		{
			if (type == ValueType.TEXT)
			{
				if (text == "true" || text == "True")
				{
					return true;
				}
				return false;
			}
			if (type == ValueType.BOOLEAN)
			{
				return boolean;
			}
			if (type == ValueType.NUMBER)
			{
				if (number != 0f)
				{
					return true;
				}
				return false;
			}
			if (type == ValueType.NEURON)
			{
				if (neuron == null)
				{
					return false;
				}
				return true;
			}
			throw new Exception("Invalid type");
		}

		public void setNeuron(string path)
		{
			setNeuron(path, false);
		}

		public void setNeuron(string path, bool noChange)
		{
			Neuron neuron = parent.getRoot().find(path);
			if (type == ValueType.NEURON)
			{
				if (this.neuron == neuron)
				{
					return;
				}
				this.neuron = neuron;
			}
			else
			{
				this.neuron = neuron;
				type = ValueType.NEURON;
			}
			if (!noChange)
			{
				change();
			}
		}

		public void setNeuron(Neuron neuron)
		{
			setNeuron(neuron, false);
		}

		public void setNeuron(Neuron neuron, bool noChange)
		{
			if (type != ValueType.NEURON || neuron != this.neuron)
			{
				type = ValueType.NEURON;
				this.neuron = neuron;
				if (!noChange)
				{
					change();
				}
			}
		}

		public float getNumber()
		{
			if (type == ValueType.TEXT)
			{
				try
				{
					return float.Parse(text);
				}
				catch (Exception)
				{
				}
				return 0f;
			}
			if (type == ValueType.BOOLEAN)
			{
				if (boolean)
				{
					return 1f;
				}
				return 0f;
			}
			if (type == ValueType.NUMBER)
			{
				return number;
			}
			if (type == ValueType.NEURON)
			{
				if (neuron == null)
				{
					return 0f;
				}
				return 1f;
			}
			throw new Exception("Invalid type");
		}

		public Neuron getNeuron()
		{
			if (type == ValueType.TEXT)
			{
				return textToNeuron(text);
			}
			if (type == ValueType.BOOLEAN)
			{
				return null;
			}
			if (type == ValueType.NUMBER)
			{
				return null;
			}
			if (type == ValueType.NEURON)
			{
				return neuron;
			}
			throw new Exception("Invalid type");
		}

		public string getType()
		{
			if (type == ValueType.TEXT)
			{
				return "Text";
			}
			if (type == ValueType.BOOLEAN)
			{
				return "Boolean";
			}
			if (type == ValueType.NUMBER)
			{
				return "Number";
			}
			if (type == ValueType.NEURON)
			{
				return "Neuron";
			}
			throw new Exception("Invalid type");
		}
	}
}
