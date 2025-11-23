using System.Linq.Expressions;

namespace GenericApiProject.Models.GenericModels;

public class GenericServiceRequest<T>
{
    public Expression<Func<T, bool>>? Expression { get; set; } = null;
    public string? IncludeProperties { get; set; } = null;
    public bool NoTracking { get; set; } = false;
    public CancellationToken CancellationToken { get; set; } = CancellationToken.None;
}