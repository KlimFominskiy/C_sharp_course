namespace Cart;

/// <summary>
/// ������� ������ ������.
/// </summary>
public abstract class Product
{
    /// <summary>
    /// Id.
    /// </summary>
    public uint Id { get; set; }
    /// <summary>
    /// ��������.
    /// </summary>
    public string Name { get; set; }
    /// <summary>
    /// ���.
    /// </summary>
    public double Weight { get; set; }
    /// <summary>
    /// ����.
    /// </summary>
    public abstract double Price { get; set; }
    /// <summary>
    /// ���� ��������.
    /// </summary>
    public abstract double TimeOfArrival { get; set; }
}