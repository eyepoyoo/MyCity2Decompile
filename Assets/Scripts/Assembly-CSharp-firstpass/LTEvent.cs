using System;

// Token: 0x0200001C RID: 28
public class LTEvent
{
	// Token: 0x060001C2 RID: 450 RVA: 0x0000CA88 File Offset: 0x0000AC88
	public LTEvent(int id, object data)
	{
		this.id = id;
		this.data = data;
	}

	// Token: 0x04000119 RID: 281
	public int id;

	// Token: 0x0400011A RID: 282
	public object data;
}
