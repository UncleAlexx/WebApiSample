namespace EfCoreSample.Validation;

public record NonRequiredMemberError(in string Name, in string Message);