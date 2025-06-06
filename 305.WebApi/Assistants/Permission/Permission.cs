namespace _305.WebApi.Assistants.Permission;

[AttributeUsage(AttributeTargets.Method)]
public class Permission(string name) : Attribute
{
	public string Name { get; } = name;
}
