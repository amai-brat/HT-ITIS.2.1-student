using System.ComponentModel.DataAnnotations;
using System.Reflection;
using System.Text.RegularExpressions;
using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Hw7.MyHtmlServices;

public static class MyEditorForModelCreator
{
    public static IHtmlContent CreateHtmlContent(Type modelType, object? model)
    {
        var properties = modelType.GetProperties();
        
        IHtmlContentBuilder builder = new HtmlContentBuilder();
        foreach (var property in properties)
        {
            builder.AppendHtml(ProcessProperty(property, model));
        }

        return builder;
    }

    private static IHtmlContent ProcessProperty(PropertyInfo property, object? model)
    {
        var container = new TagBuilder("div");
        container.InnerHtml.AppendHtml(CreateLabel(property));
        container.InnerHtml.AppendFormat("<br/>");
        container.InnerHtml.AppendHtml(property.PropertyType.IsEnum 
            ? CreateSelect(property) 
            : CreateInput(property, model));
        container.InnerHtml.AppendHtml(CreateSpan(property, model));

        return container;
    }

    private static IHtmlContent CreateLabel(PropertyInfo property)
    {
        var label = new TagBuilder("label");
        label.Attributes.Add("for", property.Name);

        var display = property.GetCustomAttribute<DisplayAttribute>();
        
        label.InnerHtml.AppendHtmlLine(
            display?.Name 
            ?? GetStringSplittedBySpacesFromCamelCase(property.Name));
        
        return label;
    }

    private static IHtmlContent CreateInput(PropertyInfo property, object? model)
    {
        var input = new TagBuilder("input");
        input.Attributes.Add("id", property.Name);
        input.Attributes.Add("name", property.Name);
        input.Attributes.Add("value", model == null 
            ? ""
            : property.GetValue(model)?.ToString());
        
        if (property.PropertyType == typeof(int))
            input.Attributes.Add("type", "number");
        else if (property.PropertyType == typeof(string))
            input.Attributes.Add("type", "text");

        return input;
    }

    private static IHtmlContent CreateSelect(PropertyInfo property)
    {
        var select = new TagBuilder("select");
        
        foreach (var enumName in property.PropertyType.GetEnumNames())
        {
            var option = new TagBuilder("option");
            option.Attributes.Add("value", enumName);
            option.InnerHtml.AppendFormat(enumName);
            select.InnerHtml.AppendHtml(option);
        }

        return select;
    }

    private static IHtmlContent CreateSpan(PropertyInfo property, object? model)
    {
        var span = new TagBuilder("span");
        if (model == null) return span;
        
        foreach (var valAttribute in property.GetCustomAttributes<ValidationAttribute>())
        {
            var result = valAttribute.GetValidationResult(
                property.GetValue(model), 
                new ValidationContext(model));
            span.InnerHtml.AppendFormat(result == null ? "" : result.ToString());
        }

        return span;
    }
    
    private static string GetStringSplittedBySpacesFromCamelCase(string camelCase)
    {
        return Regex.Replace(camelCase, "([A-Z])", " $1").Trim();
    }
}