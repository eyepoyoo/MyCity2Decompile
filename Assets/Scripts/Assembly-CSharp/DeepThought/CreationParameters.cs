using LitJson;

namespace DeepThought
{
	public class CreationParameters
	{
		private const int ChangeOffset = 3;

		private const int CreationMask = 7;

		private const int ChangeMask = 248;

		private byte _params;

		public object param;

		private JsonReader reader;

		public CreationParameters()
		{
			_params = (byte)((1 << CreationMode.ADD) | (1 << ChangeMode.CHANGE + 3));
			reader = null;
		}

		public JsonReader getReader()
		{
			return reader;
		}

		public void setReader(JsonReader reader)
		{
			this.reader = reader;
		}

		public int getCreationMode()
		{
			int num = 7 & _params;
			int num2 = 0;
			while (num > 0)
			{
				num2++;
				num >>= 1;
			}
			return num2;
		}

		public void setCreationMode(int creationMode)
		{
			_params = (byte)((1 << creationMode) | (_params & 0xF8));
		}

		public int getChangeMode()
		{
			int num = _params >> 3;
			int num2 = 0;
			while (num > 0)
			{
				num2++;
				num >>= 1;
			}
			return num2;
		}

		public void setChangeMode(int changeMode)
		{
			_params = (byte)((_params & 7) | (1 << changeMode));
		}
	}
}
