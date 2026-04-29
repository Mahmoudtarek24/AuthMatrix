namespace AuthMatrix.Models.UserModel
{
	public enum PermissionScope
	{
		All = 1,      // كل الداتا
		Own = 2,      // بياناته فقط
		Department = 3, // قسمه
		Custom = 4    // حسب الـ Filter
	}
}
