using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Hw7.MyHtmlServices;

public static class HtmlHelperExtensions
{
    public static IHtmlContent MyEditorForModel(this IHtmlHelper helper)
    {
        var model = helper.ViewData.Model;
        var modelType = helper.ViewData.ModelMetadata.ModelType;
        return MyEditorForModelCreator.CreateHtmlContent(modelType, model!);
    }

    
} 