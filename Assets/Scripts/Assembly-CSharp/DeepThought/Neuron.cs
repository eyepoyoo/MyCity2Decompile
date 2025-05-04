using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using LitJson;
using UnityEngine;

namespace DeepThought
{
	public class Neuron : IEnumerable<Neuron>, IEnumerable
	{
		protected enum MergeSetting
		{
			Merge = 0,
			MergeAndOverrideValue = 1,
			Skip = 2,
			Create = 3
		}

		internal class CreationFrame
		{
			public Neuron node;

			public string arrayType;

			public bool nodelevel = true;

			public CreationFrame(Neuron node_, string arrayType_, bool nodelevel_)
			{
				node = node_;
				arrayType = arrayType_;
				nodelevel = nodelevel_;
			}
		}

		public const string NEURON_STRING = "Neuron";

		public const string TICK_STRING = "tick";

		public const string INITIALISE_STRING = "initialise";

		public const string ENABLED_STRING = "enabled";

		public const string ACTIVATE_STRING = "activate";

		public const string DEACTIVATE_STRING = "deactivate";

		public const string INTERACTIVE_STRING = "interactive";

		public const string NONINTERACTIVE_STRING = "noninteractive";

		public const string SETALIVE_STRING = "setAlive";

		public const string SETVALUE_STRING = "setValue";

		public const string PROPAGATE_STRING = "propagate";

		public const string UP_STRING = "up";

		public const string DOWN_STRING = "down";

		public const string TARGET_STRING = "target";

		public const string EMPTY_STRING = "";

		protected Neuron parent;

		protected string name;

		protected string type;

		protected Value value;

		protected bool alive;

		protected MergeSetting merges;

		protected bool leaf;

		public static bool cheapMode;

		protected AmuzoBetterList<Neuron> children;

		private static CreationParameters blankCreationParameters;

		public Neuron this[int index]
		{
			get
			{
				return (children != null) ? children[index] : null;
			}
		}

		public int numChildren
		{
			get
			{
				if (children == null)
				{
					return 0;
				}
				return children.Count;
			}
		}

		public Value Value
		{
			get
			{
				return value;
			}
		}

		public string Text
		{
			get
			{
				if (Value == null)
				{
					return string.Empty;
				}
				return Value.getText();
			}
			set
			{
				getValue(true).setText(value);
			}
		}

		public float Number
		{
			get
			{
				if (Value == null)
				{
					return 0f;
				}
				return Value.getNumber();
			}
			set
			{
				getValue(true).setNumber(value);
			}
		}

		public bool Boolean
		{
			get
			{
				if (Value == null)
				{
					return false;
				}
				return Value.getBoolean();
			}
			set
			{
				getValue(true).setBoolean(value);
			}
		}

		public Neuron NeuronRef
		{
			get
			{
				if (Value == null)
				{
					return null;
				}
				return Value.getNeuron();
			}
			set
			{
				getValue(true).setNeuron(value);
			}
		}

		public bool Alive
		{
			get
			{
				return alive;
			}
		}

		public Neuron(Neuron parent, CreationParameters parameters)
		{
			this.parent = parent;
			type = "Neuron";
			value = null;
			alive = true;
			leaf = false;
			setName("untitled");
			merges = MergeSetting.Create;
			if (parent != null && parameters.getCreationMode() != CreationMode.NO_ADD)
			{
				parent.initchildren();
				parent.children.Add(this);
			}
		}

		IEnumerator<Neuron> IEnumerable<Neuron>.GetEnumerator()
		{
			initchildren();
			return children.GetEnumerator();
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			initchildren();
			return children.GetEnumerator();
		}

		protected void initchildren()
		{
			if (children == null)
			{
				children = new AmuzoBetterList<Neuron>(4);
			}
		}

		~Neuron()
		{
			Dispose(false);
		}

		protected virtual void Dispose(bool disposing)
		{
		}

		public void Dispose()
		{
			if (children != null)
			{
				while (children.Count > 0)
				{
					children[0].Dispose();
					children.RemoveAt(0);
				}
			}
			Dispose(true);
			GC.SuppressFinalize(this);
		}

		private static Neuron parse(CreationParameters parameters, Neuron parent)
		{
			JsonReader reader = parameters.getReader();
			if (reader == null)
			{
				return null;
			}
			Neuron neuron = null;
			string text = null;
			AmuzoBetterStack<CreationFrame> amuzoBetterStack = new AmuzoBetterStack<CreationFrame>(50);
			CreationFrame creationFrame = new CreationFrame(null, null, true);
			amuzoBetterStack.Push(creationFrame);
			try
			{
				while (reader.Read())
				{
					switch (reader.Token)
					{
					case JsonToken.PropertyName:
						if (creationFrame.nodelevel)
						{
							string arrayType = reader.Value.ToString();
							creationFrame = new CreationFrame(create(arrayType, string.Empty, null, false, parameters, creationFrame.node), null, false);
							amuzoBetterStack.Push(creationFrame);
							if (neuron == null)
							{
								neuron = creationFrame.node;
							}
							reader.Read();
							if (reader.Token == JsonToken.ArrayStart)
							{
								creationFrame.arrayType = arrayType;
								reader.Read();
							}
						}
						else
						{
							text = parseProperty(creationFrame.node, parameters);
							if (reader.Token == JsonToken.ObjectStart)
							{
								creationFrame = new CreationFrame(create(text, string.Empty, null, false, parameters, creationFrame.node), null, false);
								amuzoBetterStack.Push(creationFrame);
							}
							else if (reader.Token == JsonToken.ArrayStart)
							{
								creationFrame = new CreationFrame(create(text, string.Empty, null, false, parameters, creationFrame.node), text, false);
								amuzoBetterStack.Push(creationFrame);
								reader.Read();
							}
						}
						break;
					case JsonToken.ObjectStart:
						if (creationFrame.arrayType != null)
						{
							string arrayType2 = creationFrame.arrayType;
							creationFrame = amuzoBetterStack.Pop();
							creationFrame = new CreationFrame(create(arrayType2, string.Empty, null, false, parameters, creationFrame.node), null, false);
							amuzoBetterStack.Push(creationFrame);
							creationFrame.arrayType = arrayType2;
						}
						break;
					case JsonToken.ObjectEnd:
						if (creationFrame.arrayType == null)
						{
							creationFrame = amuzoBetterStack.Pop();
						}
						creationFrame.nodelevel = true;
						creationFrame.node = ((creationFrame.node != null) ? creationFrame.node.parent : null);
						break;
					case JsonToken.ArrayStart:
						Debug.LogWarning("ArrayStart: Should not get here");
						break;
					case JsonToken.ArrayEnd:
						creationFrame.nodelevel = true;
						break;
					}
				}
			}
			catch (Exception ex)
			{
				Debug.LogError(ex.Message);
			}
			return neuron;
		}

		private static string parseProperty(Neuron node, CreationParameters parameters)
		{
			JsonReader reader = parameters.getReader();
			string text = reader.Value.ToString();
			reader.Read();
			string text2 = ((reader.Value != null) ? reader.Value.ToString() : null);
			switch (text)
			{
			case "name":
				node.name = text2;
				break;
			case "value":
				node.value = new Value(node, text2);
				break;
			case "values":
			{
				string[] array = text2.Split(',');
				for (int i = 0; i < array.Length; i++)
				{
					node.addChild("Neuron", string.Format("value_{0}", i), array[i], true);
				}
				break;
			}
			default:
				if (reader.Token == JsonToken.String)
				{
					node.addChild("Neuron", text, text2, true);
				}
				break;
			case "merge":
				break;
			}
			return text;
		}

		public static Neuron create(string type, string name, string value = null, bool isLeaf = false, CreationParameters param = null, Neuron parent = null)
		{
			if (blankCreationParameters == null)
			{
				blankCreationParameters = new CreationParameters();
			}
			Neuron neuron = NeuronFactoryStore.get(type).create(parent, param ?? blankCreationParameters);
			neuron.type = type;
			neuron.setName(name);
			neuron.value = ((value != null) ? new Value(neuron, value) : null);
			neuron.leaf = isLeaf;
			return neuron;
		}

		public static Neuron generate(string json, object param = null)
		{
			return generate(null, json, param);
		}

		public static Neuron generate(Neuron parent, string json, object param = null)
		{
			CreationParameters creationParameters = new CreationParameters();
			creationParameters.param = param;
			JsonReader reader = new JsonReader(json);
			creationParameters.setReader(reader);
			creationParameters.setCreationMode(CreationMode.ADD);
			return parse(creationParameters, parent);
		}

		public string serialize()
		{
			Debug.LogWarning("Neuron serialize is not yet refactored for JSON");
			return string.Empty;
		}

		public void getChildren(List<Neuron> children)
		{
			if (this.children != null)
			{
				Debug.LogWarning("getChildren: creating reference copy of children list -> do you want to do this?");
				for (int i = 0; i < this.children.Count; i++)
				{
					children.Add(this.children[i]);
				}
			}
		}

		public void getChildren(string type, List<Neuron> children)
		{
			if (children == null)
			{
				return;
			}
			for (int i = 0; i < this.children.Count; i++)
			{
				if (this.children[i].getType() == type)
				{
					children.Add(this.children[i]);
				}
			}
		}

		public Neuron getChild(string name)
		{
			return tryGetChild(name);
		}

		public Neuron getChild(int index)
		{
			if (children == null || index >= children.Count)
			{
				throw new Exception("Failed to find child at \"" + index + "\" within \"" + name + "\"");
			}
			return children[index];
		}

		public Neuron tryGetChild(string name)
		{
			if (children == null)
			{
				return null;
			}
			for (int num = children.size - 1; num >= 0; num--)
			{
				if (children.buffer[num].name == name)
				{
					if (num == children.size - 1)
					{
						return children.buffer[num];
					}
					Neuron neuron = children.buffer[num];
					children.buffer[num] = children.buffer[num + 1];
					children.buffer[num + 1] = neuron;
					return neuron;
				}
			}
			return null;
		}

		public Value tryGetValue(int index)
		{
			Neuron neuron = tryGetChild(string.Format("value_{0}", index));
			if (neuron != null)
			{
				return neuron.Value;
			}
			return null;
		}

		public bool containsChild(string name)
		{
			if (children == null)
			{
				return false;
			}
			for (int i = 0; i < children.size; i++)
			{
				if (children.buffer[i].name == name)
				{
					return true;
				}
			}
			return false;
		}

		public Value getValue()
		{
			return value;
		}

		public Value getValue(bool create)
		{
			if (value == null && create)
			{
				value = new Value(this, string.Empty);
			}
			return Value;
		}

		public string getName()
		{
			return name;
		}

		public string getType()
		{
			return type;
		}

		public void setName(string name)
		{
			if (!(name == this.name))
			{
				if (parent == null)
				{
					this.name = name;
				}
				else if (parent.containsChild(name))
				{
					this.name = getUniqueName(this);
				}
				else
				{
					this.name = name;
				}
			}
		}

		private static string getUniqueName(Neuron node)
		{
			return string.Format("{0}_{1}", node.name, DateTime.Now.Ticks);
		}

		public Neuron getRoot()
		{
			if (parent == null)
			{
				return this;
			}
			return parent.getRoot();
		}

		public string getFullName()
		{
			return getFullName(string.Empty);
		}

		private string getFullName(string current)
		{
			if (parent == null)
			{
				if (current.Length > 0)
				{
					return string.Format("{0}/{1}", name, current);
				}
				return name;
			}
			if (current == string.Empty)
			{
				return parent.getFullName(name);
			}
			return parent.getFullName(string.Format("{0}/{1}", name, current));
		}

		public string getPath()
		{
			return getDynamicPath();
		}

		public string getDynamicPath()
		{
			StringBuilder stringBuilder = new StringBuilder();
			Neuron neuron = null;
			string text = null;
			neuron = this;
			while (neuron != null)
			{
				if (neuron.getParent() == null)
				{
					neuron = neuron.parent;
					continue;
				}
				if (text != null)
				{
					stringBuilder.Insert(0, text);
				}
				stringBuilder.Insert(0, neuron.name);
				neuron = neuron.parent;
				text = "/";
			}
			return stringBuilder.ToString();
		}

		public virtual bool onChange(string changePath)
		{
			return false;
		}

		public virtual void onChange(int type, Neuron neuron)
		{
		}

		public void change(string changePath)
		{
		}

		public void change(int type, Neuron neuron)
		{
		}

		public Neuron add(string json, object param = null)
		{
			if (leaf)
			{
				throw new Exception("Cannot add json \"" + json + "\" to \"" + name + "\" because it is a leaf node");
			}
			return generate(this, json, param);
		}

		public Neuron add(Neuron neuron)
		{
			if (neuron.parent != null)
			{
				throw new Exception("\"" + neuron.name + "\" is already part of another hierarchy");
			}
			initchildren();
			if (containsChild(neuron.name))
			{
				neuron.name = getUniqueName(neuron);
			}
			children.Add(neuron);
			neuron.parent = this;
			return neuron;
		}

		public Neuron reparent(Neuron neuron)
		{
			if (neuron.parent != null)
			{
				neuron.parent.children.Remove(neuron);
				neuron.parent = null;
			}
			return add(neuron);
		}

		public Neuron message(string json)
		{
			return null;
		}

		public Neuron message(Neuron messageItem)
		{
			return null;
		}

		public virtual void onMessage(Neuron messageItem, Neuron resultItem)
		{
		}

		public Neuron tryFind(string name)
		{
			if (children == null)
			{
				return null;
			}
			Neuron neuron = tryGetChild(name);
			if (neuron == null)
			{
				for (int num = children.Count - 1; num >= 0; num--)
				{
					neuron = children[num].tryFind(name);
					if (neuron != null)
					{
						return neuron;
					}
				}
			}
			return neuron;
		}

		public Neuron find(string name)
		{
			Neuron neuron = null;
			neuron = tryFind(name);
			if (neuron == null)
			{
				throw new Exception("Failed to find Neuron \"" + name + "\"");
			}
			return neuron;
		}

		public void merge(string json)
		{
			Debug.LogError("This merge functionality is deprecated");
		}

		public void merge(Neuron mergeNeuron)
		{
			Debug.LogError("This merge functionality is deprecated");
		}

		public virtual Neuron getNeuron(string name)
		{
			string[] array = null;
			Neuron neuron = null;
			int num = 0;
			string empty = string.Empty;
			Neuron neuron2 = null;
			array = name.Split('/');
			neuron = this;
			if (array.Length > 0 && array[0] == getRoot().getName())
			{
				neuron = getRoot();
				num = 1;
			}
			else if (array.Length > 0 && array[0] == string.Empty)
			{
				neuron = getRoot();
				num = 1;
			}
			else if ((name.Contains("--") && name.Substring(0, 2) == "--") || (name.Contains("-+") && name.Substring(0, 2) == "-+"))
			{
				neuron2 = this;
				empty = name.Split('/')[0].Substring(2);
				num = 1;
				while (neuron2 != getRoot())
				{
					neuron2 = neuron2.parent;
					if (neuron2.name == empty)
					{
						neuron = neuron2;
						break;
					}
				}
			}
			else if (name.Contains("++") && name.Substring(0, 2) == "++")
			{
				empty = name.Split('/')[0].Substring(2);
				num = 1;
				neuron2 = tryFind(empty);
				if (neuron2 != null)
				{
					neuron = neuron2;
				}
			}
			else if (name.Contains("..") && name.Substring(0, 2) == "..")
			{
				neuron = neuron.getParent();
				num = 1;
			}
			for (int i = num; i < array.Length; i++)
			{
				neuron = ((!(array[i] == "..")) ? neuron.getChild(array[i]) : neuron.getParent());
			}
			if (neuron == this)
			{
				return null;
			}
			return neuron;
		}

		public Neuron tryGetNeuron(string name)
		{
			return getNeuron(name);
		}

		public Neuron getParent()
		{
			return parent;
		}

		public Neuron getChild(string name, bool create)
		{
			Neuron neuron = null;
			neuron = tryGetChild(name);
			if (neuron == null)
			{
				if (!create)
				{
					return null;
				}
				neuron = addChild(name);
			}
			return neuron;
		}

		public Neuron getNeuron(string name, bool create)
		{
			string[] array = null;
			Neuron neuron = null;
			int num = 0;
			array = name.Split('/');
			neuron = this;
			if (array.Length > 0 && array[0] == getRoot().getName())
			{
				num = 1;
			}
			for (int i = num; i < array.Length; i++)
			{
				neuron = ((!(array[i] == "..")) ? neuron.getChild(array[i], create) : neuron.getParent());
			}
			return neuron;
		}

		public void setValue(string path, bool value)
		{
			getNeuron(path, true).getValue(true).setBoolean(value);
		}

		public void setValue(string path, float value)
		{
			getNeuron(path, true).getValue(true).setNumber(value);
		}

		public void setValue(string path, string value)
		{
			getNeuron(path, true).getValue(true).setText(value);
		}

		public void setValue(string path, Neuron value)
		{
			getNeuron(path, true).getValue(true).setNeuron(value);
		}

		public void makeLeaf()
		{
			if (children != null && children.Count > 0)
			{
				throw new Exception("Cannot make \"" + name + "\" a leaf node since it has children");
			}
			leaf = true;
		}

		public bool getBoolean(string path)
		{
			Neuron neuron = null;
			neuron = getNeuron(path);
			if (neuron.Value == null)
			{
				return false;
			}
			return neuron.Value.getBoolean();
		}

		public bool tryGetBoolean(Neuron neuron, bool defaultVal)
		{
			Neuron neuron2 = null;
			neuron2 = neuron;
			if (neuron2.Value == null)
			{
				return defaultVal;
			}
			return neuron2.Value.getBoolean();
		}

		public bool tryGetBoolean(string path, bool defaultVal)
		{
			Neuron neuron = null;
			neuron = tryGetNeuron(path);
			if (neuron == null)
			{
				return defaultVal;
			}
			if (neuron.Value == null)
			{
				return defaultVal;
			}
			return neuron.Value.getBoolean();
		}

		public double getNumber(string path)
		{
			Neuron neuron = null;
			neuron = getNeuron(path);
			if (neuron.Value == null)
			{
				return 0.0;
			}
			return neuron.Value.getNumber();
		}

		public string getText(string path)
		{
			Neuron neuron = null;
			neuron = getNeuron(path);
			if (neuron.Value == null)
			{
				return string.Empty;
			}
			return neuron.Value.getText();
		}

		public Neuron getNeuronRef(string path)
		{
			Neuron neuron = null;
			neuron = getNeuron(path);
			if (neuron.Value == null)
			{
				return null;
			}
			return neuron.Value.getNeuron();
		}

		public Neuron addChild(string type, string name, string value, bool isLeaf = false)
		{
			return add(create(type, name, value, isLeaf));
		}

		public Neuron addChild(string name, string value)
		{
			return addChild("Neuron", name, value);
		}

		public Neuron addChild(string name)
		{
			return addChild(name, null);
		}

		public void kill()
		{
			alive = false;
			if (parent != null)
			{
				parent.children.Remove(this);
			}
		}

		public bool getAlive()
		{
			return alive;
		}
	}
}
