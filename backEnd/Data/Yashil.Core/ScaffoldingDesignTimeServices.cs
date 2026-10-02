using System;
using Microsoft.EntityFrameworkCore.Design;
using System.Collections.Generic;
using HandlebarsDotNet;
using Microsoft.Extensions.DependencyInjection;

/*
 * https://github.com/TrackableEntities/EntityFrameworkCore.Scaffolding.Handlebars
 * dotnet ef dbcontext scaffold "Data Source=<server>;Initial Catalog=<database>;User ID=<user>;Password=<password>;MultipleActiveResultSets=True;Trust Server Certificate=True" Microsoft.EntityFrameworkCore.SqlServer -o Entities -c  YashilAppDbContext -f --context-dir Data --no-pluralize --no-build
 */
namespace Yashil.Core
{
    public class ScaffoldingDesignTimeServices : IDesignTimeServices
    {


        protected virtual ReverseEngineerOptions GetReverseEngineerOptions()
        {
            return ReverseEngineerOptions.EntitiesOnly;
        }
        protected virtual List<string> GetExcludedTables()
        {
            return new List<string> { "AdOrganization", "Resume1", "wrongSataSubsidy", "TmpPaymentForPaymentCatgoryUpdate", "CardNumberOrganization","Naja", "FaniStandardCategory", "Schemas", "FanCity", "TmpFcource", "BackupCategory", "TmpCategory", "TmpCity", "OldVocher", "MissedOrderItemPriceRightShare", "Version" };
        }

        
        public void ConfigureDesignTimeServices(IServiceCollection services)
        {
            // Uncomment to launch JIT debugger and hit breakpoints
            // Debugger.Launch();

            // Add Handlebars scaffolding templates
            services.AddHandlebarsScaffolding(options =>
            {
                // Generate both context and entities
                options.ReverseEngineerOptions = GetReverseEngineerOptions();

                // Enable Nullable reference types Support https://docs.microsoft.com/en-us/ef/core/miscellaneous/nullable-reference-types
                // options.EnableNullableReferenceTypes = true;

                // Put Models into folders by DB Schema
                options.EnableSchemaFolders = false;
                // Exclude some tables
                options.ExcludedTables = GetExcludedTables();
                // Add custom template data
                options.TemplateData = new Dictionary<string, object>
                {
                    { "models-namespace", "Yashil.Core.Entities" }
            };

                // Disable comments generation
                options.GenerateComments = true;
            });

            // Register Handlebars helper
            //  var baseClassHelper = (helperName: "base-class-helper", helperFunction: (Action<TextWriter, Dictionary<string, object>, object[]>)BaseClassHelper);
            // Register Handlebars block helper
            //var ifCondHelper =
            //    (helperName: "ifCond",
            //        helperFunction:
            //        (Action<TextWriter, HelperOptions, Dictionary<string, object>, object[]>)MyHbsBlockHelper);


            (string helperName, Action<EncodedTextWriter, Context, Arguments> helperFunction) baseClassHelper =
                 (helperName: "base-class-helper",
                     helperFunction: BaseClassHelper);
            // Add optional Handlebars helpers
            services.AddHandlebarsHelpers(baseClassHelper);
            (string helperName, Action<EncodedTextWriter, BlockHelperOptions, Context, Arguments> helperFunction) ifCondHelper =
                (helperName: "ifCond",
                    helperFunction:
                    (Action<EncodedTextWriter, BlockHelperOptions, Context, Arguments>)MyHbsBlockHelper);
            services.AddHandlebarsBlockHelpers(ifCondHelper);

            // Add Handlebars transformer for Country property
            services.AddHandlebarsTransformers(

            //entityNameTransformer: n =>
            //{
            //    return n;
            //}
            );
        }

        // Sample Handlebars helper
        void BaseClassHelper(EncodedTextWriter writer, Context context, Arguments arguments)
        {
            var hasApplicationId = false;
            var hasCreatorOrganizationId = false;
            var hasCreateBy = false;
            var hasModifyBy = false;
            var hasDeleted = false;
            var hasCreationDate = false;
            var hasModificationDate = false;
            string baseClasses = "";

            var c = context.GetValue<List<Dictionary<string, object>>>("properties");
            if (c != null)
            {
                foreach (var item in c)
                {
                    if (item.TryGetValue("property-name", out var colName))
                    {
                        switch (colName)
                        {
                            case "ApplicationId":
                                if (item.TryGetValue("nullable-reference-types", out var isNullable))
                                {
                                    if (!(bool)isNullable)
                                    {
                                        hasApplicationId = true;
                                    }
                                }
                                break;
                            case "CreatorOrganizationId":
                                hasCreatorOrganizationId = true;
                                break;
                            case "CreateBy":
                                hasCreateBy = true;
                                break;
                            case "ModifyBy":
                                hasModifyBy = true;
                                break;
                            case "Deleted":
                                hasDeleted = true;
                                break;
                            case "CreationDate":
                                hasCreationDate = true;
                                break;
                            case "ModificationDate":
                                hasModificationDate = true;
                                break;
                        }
                    }
                }

                if (hasCreateBy && hasModifyBy && hasDeleted && hasCreationDate && hasModificationDate)
                {
                    baseClasses = @":IBaseEntity<int>";
                }
                if (hasApplicationId && hasCreatorOrganizationId)
                {
                    if (string.IsNullOrEmpty(baseClasses))
                    {
                        baseClasses = ":";
                    }
                    else
                    {
                        baseClasses += ",";
                    }
                    baseClasses += "IApplicationBasedEntity";
                }
            }
            writer.WriteSafeString(baseClasses);
        }

        // Sample Handlebars block helper
        void MyHbsBlockHelper(EncodedTextWriter writer, BlockHelperOptions options, Context context, Arguments args)
        {
            var val0Str = args[0]?.ToString();
            var val1Str = args[1]?.ToString();
            var val2Str = args[2]?.ToString();
            var val1 = float.Parse(val0Str ?? "0");
            var val2 = float.Parse(val2Str ?? "0");

            switch (val1Str)
            {
                case ">":
                    if (val1 > val2)
                        options.Template(writer, context);
                    else
                        options.Inverse(writer, context);
                    break;
                case "=":
                case "==":
                    if (val1 == val2)
                        options.Template(writer, context);
                    else
                        options.Inverse(writer, context);
                    break;
                case "<":
                    if (val1 < val2)
                        options.Template(writer, context);
                    else
                        options.Inverse(writer, context);
                    break;
                case "!=":
                case "<>":
                    if (val1 != val2)
                        options.Template(writer, context);
                    else
                        options.Inverse(writer, context);
                    break;
            }
        }
    }
}
