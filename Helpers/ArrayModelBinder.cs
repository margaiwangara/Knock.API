using System;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace Knock.API.Helpers
{
  public class ArrayModelBinder : IModelBinder
  {
    public Task BindModelAsync(ModelBindingContext bindingContext)
    {
      // binder works on enumerable types
      if(!bindingContext.ModelMetadata.IsEnumerableType)
      {
        bindingContext.Result = ModelBindingResult.Failed();
        return Task.CompletedTask;
      }

      // get values throught value provided then convert to string
      var value = bindingContext.ValueProvider
                      .GetValue(bindingContext.ModelName).ToString();

      // if value is null or whitespace return null
      if(string.IsNullOrWhiteSpace(value))
      {
        bindingContext.Result = ModelBindingResult.Success(null);
        return Task.CompletedTask;
      }

      // if success
      var elementType = bindingContext.ModelType.GetTypeInfo().GenericTypeArguments[0];
      var converter = TypeDescriptor.GetConverter(elementType);

      // split value
      var values = value.Split( new []{ "," }, StringSplitOptions.RemoveEmptyEntries)
                    .Select(x => converter.ConvertFromString(x.Trim()))
                    .ToArray();

      // Create an array of that type, and set it as the Model value 
      var typedValues = Array.CreateInstance(elementType, values.Length);
      values.CopyTo(typedValues, 0);
      bindingContext.Model = typedValues;

      // return a successful result, passing in the Model 
      bindingContext.Result = ModelBindingResult.Success(bindingContext.Model);
      return Task.CompletedTask;

    }
  }
}