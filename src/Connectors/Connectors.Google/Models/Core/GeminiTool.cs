// Copyright (c) Richasy. All rights reserved.
// Licensed under the MIT License.

using System.Text.Json.Serialization;

namespace Richasy.AgentKernel.Connectors.Google.Models.Core;

internal sealed class GeminiTool
{
    /// <summary>
    /// A list of FunctionDeclarations available to the model that can be used for function calling.
    /// </summary>
    /// <remarks>
    /// The model or system does not execute the function. Instead the defined function may be returned as a
    /// [FunctionCall][content.part.function_call] with arguments to the client side for execution.
    /// The model may decide to call a subset of these functions by populating
    /// [FunctionCall][content.part.function_call] in the response. The next conversation turn may contain
    /// a [FunctionResponse][content.part.function_response] with the [content.role] "function" generation context for the next model turn.
    /// </remarks>
    [JsonPropertyName("function_declarations")]
    public IList<FunctionDeclaration> Functions { get; set; } = [];

    /// <summary>
    /// Structured representation of a function declaration as defined by the OpenAPI 3.03 specification.
    /// Included in this declaration are the function name and parameters.
    /// This FunctionDeclaration is a representation of a block of code that can be used as a Tool by the model and executed by the client.
    /// </summary>
    internal sealed class FunctionDeclaration
    {
        /// <summary>
        /// Required. Name of function.
        /// </summary>
        /// <remarks>
        /// Must be a-z, A-Z, 0-9, or contain underscores and dashes, with a maximum length of 63.
        /// </remarks>
        [JsonPropertyName("name")]
        public string Name { get; set; } = null!;

        /// <summary>
        /// Required. A brief description of the function.
        /// </summary>
        [JsonPropertyName("description")]
        public string Description { get; set; } = null!;

        /// <summary>
        /// Optional. Describes the parameters to this function.
        /// Reflects the Open API 3.03 Parameter Object string Key: the name of the parameter.
        /// Parameter names are case sensitive. Schema Value: the Schema defining the type used for the parameter.
        /// </summary>
        [JsonPropertyName("parameters")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        [JsonConverter(typeof(BinaryJsonSchemaConverter))]
        public BinaryData? Parameters { get; set; }
    }
}
