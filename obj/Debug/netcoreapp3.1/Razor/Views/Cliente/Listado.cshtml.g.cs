#pragma checksum "C:\Users\david\OneDrive\Escritorio\2020-02\Multiplataforma\DocWeb_Prueba\DocWeb_Prueba\Views\Cliente\Listado.cshtml" "{ff1816ec-aa5e-4d10-87f7-6f4963833460}" "3398cb0415605850f0913df9ebf037e9e9a961b0"
// <auto-generated/>
#pragma warning disable 1591
[assembly: global::Microsoft.AspNetCore.Razor.Hosting.RazorCompiledItemAttribute(typeof(AspNetCore.Views_Cliente_Listado), @"mvc.1.0.view", @"/Views/Cliente/Listado.cshtml")]
namespace AspNetCore
{
    #line hidden
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.Rendering;
    using Microsoft.AspNetCore.Mvc.ViewFeatures;
#nullable restore
#line 1 "C:\Users\david\OneDrive\Escritorio\2020-02\Multiplataforma\DocWeb_Prueba\DocWeb_Prueba\Views\_ViewImports.cshtml"
using DocWeb_Prueba;

#line default
#line hidden
#nullable disable
#nullable restore
#line 2 "C:\Users\david\OneDrive\Escritorio\2020-02\Multiplataforma\DocWeb_Prueba\DocWeb_Prueba\Views\_ViewImports.cshtml"
using DocWeb_Prueba.Models;

#line default
#line hidden
#nullable disable
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"3398cb0415605850f0913df9ebf037e9e9a961b0", @"/Views/Cliente/Listado.cshtml")]
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"5d130e08d4d5cf34f967f7c4aa61361befe68e8a", @"/Views/_ViewImports.cshtml")]
    public class Views_Cliente_Listado : global::Microsoft.AspNetCore.Mvc.Razor.RazorPage<List<DocWeb_Prueba.Models.Cliente>>
    {
        #pragma warning disable 1998
        public async override global::System.Threading.Tasks.Task ExecuteAsync()
        {
            WriteLiteral("<h2>Clientes</h2>\r\n<table class=\"table table-striped\">\r\n    <tr>\r\n        <th>Nombres</th>\r\n        <th>Apellido</th>\r\n        <th>Fecha de Nacimiento</th>\r\n    </tr>\r\n");
#nullable restore
#line 12 "C:\Users\david\OneDrive\Escritorio\2020-02\Multiplataforma\DocWeb_Prueba\DocWeb_Prueba\Views\Cliente\Listado.cshtml"
     foreach (var img in Model)
    {

#line default
#line hidden
#nullable disable
            WriteLiteral("        <tr>\r\n            <td>");
#nullable restore
#line 15 "C:\Users\david\OneDrive\Escritorio\2020-02\Multiplataforma\DocWeb_Prueba\DocWeb_Prueba\Views\Cliente\Listado.cshtml"
           Write(img.Nombres);

#line default
#line hidden
#nullable disable
            WriteLiteral("</td>\r\n            <td>");
#nullable restore
#line 16 "C:\Users\david\OneDrive\Escritorio\2020-02\Multiplataforma\DocWeb_Prueba\DocWeb_Prueba\Views\Cliente\Listado.cshtml"
           Write(img.Apellidos);

#line default
#line hidden
#nullable disable
            WriteLiteral("</td>\r\n            <td>");
#nullable restore
#line 17 "C:\Users\david\OneDrive\Escritorio\2020-02\Multiplataforma\DocWeb_Prueba\DocWeb_Prueba\Views\Cliente\Listado.cshtml"
           Write(img.fecha_nacimiento);

#line default
#line hidden
#nullable disable
            WriteLiteral("</td>\r\n        </tr>\r\n");
#nullable restore
#line 19 "C:\Users\david\OneDrive\Escritorio\2020-02\Multiplataforma\DocWeb_Prueba\DocWeb_Prueba\Views\Cliente\Listado.cshtml"
    }

#line default
#line hidden
#nullable disable
            WriteLiteral("\r\n</table>\r\n");
        }
        #pragma warning restore 1998
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.ViewFeatures.IModelExpressionProvider ModelExpressionProvider { get; private set; }
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.IUrlHelper Url { get; private set; }
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.IViewComponentHelper Component { get; private set; }
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.Rendering.IJsonHelper Json { get; private set; }
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.Rendering.IHtmlHelper<List<DocWeb_Prueba.Models.Cliente>> Html { get; private set; }
    }
}
#pragma warning restore 1591