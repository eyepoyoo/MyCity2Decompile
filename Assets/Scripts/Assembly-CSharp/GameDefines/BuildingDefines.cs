namespace GameDefines
{
	public class BuildingDefines
	{
		public enum EBuildingType
		{
			RESOURCE_1_GENERATOR = 0,
			RESOURCE_2_GENERATOR = 1,
			RESOURCE_3_GENERATOR = 2,
			PREMIUM_GENERATOR = 3,
			STORAGE = 4
		}

		public static EBuildingType[] _allTypes = new EBuildingType[5]
		{
			EBuildingType.RESOURCE_1_GENERATOR,
			EBuildingType.RESOURCE_2_GENERATOR,
			EBuildingType.RESOURCE_3_GENERATOR,
			EBuildingType.PREMIUM_GENERATOR,
			EBuildingType.STORAGE
		};
	}
}
