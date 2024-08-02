namespace ManeKani.Pages.Shared.Components;

public class ApiKeyPermissionTableModel(bool readOnly = false, IEnumerable<ApiKeyPermission>? initialPermissions = null)
{
    public ApiKeyPermissionTableModel(bool readOnly) : this(readOnly: readOnly, initialPermissions: null)
    { }

    public ApiKeyPermissionTableModel(IEnumerable<ApiKeyPermission>? initialPermissions) : this(readOnly: false, initialPermissions: initialPermissions)
    { }

    public bool ReadOnly { get; set; } = readOnly;
    public IEnumerable<ApiKeyPermission>? InitialPermissions { get; set; } = initialPermissions;
}

public struct ApiKeyPermission
{
    public string Name;
    public bool CanModify;
    public bool CanDelete;
}