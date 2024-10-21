using System.Text;

namespace Infrastructure.Persistence.Dapper.Core.Builder;
public class SqlTemplateBuilder
{
    private StringBuilder sqlTemplate = new();

    public SqlTemplateBuilder FromTemplate(string template)
    {
        sqlTemplate = new StringBuilder(template);
        return this;
    }

    public override string ToString() => sqlTemplate.ToString();
}
