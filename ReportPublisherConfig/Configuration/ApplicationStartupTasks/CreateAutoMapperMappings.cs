using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using ReportPublisherConfig.Handlers.Home.save;
using ReportPublisherConfig.model;

namespace ReportPublisherConfig.Configuration.ApplicationStartupTasks
{
    public class CreateAutoMapperMappings : IApplicationStartupTask
    {
        public void Execute()
        {
            Mapper.CreateMap<jsTreeSaveInputModel, configuration>()
                .ForMember(dest => dest.reportLocalPath, opt => opt.Ignore())
                .ForMember(dest => dest.policy, opt => opt.Ignore())
                .ForMember(dest => dest.folder, opt => opt.MapFrom(src => src.children));
            
            Mapper.CreateMap<jsTreeSaveInputModel, Folder>()
                .ForMember(dest => dest.inheritPermissions, opt => opt.MapFrom(src => doesAttributeExist(src, "Folder")))
                .ForMember(dest => dest.report, opt => opt.MapFrom(src => getChildrenWithAttribute(src, "Report", "CommonReport")))
                .ForMember(dest => dest.styleSheet, opt => opt.MapFrom(src => getChildrenWithAttribute(src, "StyleSheet", "CommonStyleSheet")))
                .ForMember(dest => dest.folder, opt => opt.MapFrom(src => getChildrenWithAttribute(src, "Folder", "DoesNotInheritPermissionsFolder")))
                .ForMember(dest => dest.Text, opt => opt.Ignore())
                .ForMember(dest => dest.sharedDataSource, opt => opt.MapFrom(src => getChildWithAttribute(src, "DataSource")))
                .ForMember(dest => dest.name, opt => opt.MapFrom(src => src.data));

            Mapper.CreateMap<jsTreeSaveInputModel, Report>()
                .ForMember(dest => dest.parameter, opt => opt.MapFrom(src => getChildrenWithAttributeByFlattening(src, "ParameterCollection")))
                .ForMember(dest => dest.property, opt => opt.MapFrom(src => getChildrenWithAttributeByFlattening(src, "PropertyCollection")))
                .ForMember(dest => dest.name, opt => opt.MapFrom(src => src.data))
                .ForMember(dest => dest.commonReport, opt => opt.MapFrom(src => doesAttributeExist(src, "CommonReport")));

            Mapper.CreateMap<jsTreeSaveInputModel, StyleSheet>()
                .ForMember(dest => dest.name, opt => opt.MapFrom(src => src.data))
                .ForMember(dest => dest.commonReport, opt => opt.MapFrom(src => doesAttributeExist(src, "CommonStyleSheet")));

            Mapper.CreateMap<jsTreeSaveInputModel, Parameter>()
                .ForMember(dest => dest.name, opt => opt.MapFrom(src => getKey(src)))
                .ForMember(dest => dest.value, opt => opt.MapFrom(src => getValue(src)));

            Mapper.CreateMap<jsTreeSaveInputModel, Property>()
                .ForMember(dest => dest.name, opt => opt.MapFrom(src => getKey(src)))
                .ForMember(dest => dest.value, opt => opt.MapFrom(src => getValue(src)));

            Mapper.CreateMap<jsTreeSaveInputModel, sharedDataSource>()
                .ForMember(dest => dest.name, opt => opt.MapFrom(src => src.data));

            Mapper.AssertConfigurationIsValid();
        }

        private static string getKey(jsTreeSaveInputModel src)
        {
            return src.data.Split('=')[0].Trim();
        }

        private static string getValue(jsTreeSaveInputModel src)
        {
            return src.data.Split('=')[1].Trim();
        }

        private static jsTreeSaveInputModel getChildWithAttribute(jsTreeSaveInputModel src, string attributeName)
        {
            return src.children.SingleOrDefault(x => doesAttributeExist(x, attributeName));
        }

        private static IEnumerable<jsTreeSaveInputModel> getChildrenWithAttribute(jsTreeSaveInputModel src, params string[] attributeNames)
        {
            return src.children.Where(x => attributeNames.Any(y => doesAttributeExist(x, y)));
        }

        private static IEnumerable<jsTreeSaveInputModel> getChildrenWithAttributeByFlattening(jsTreeSaveInputModel src, string attributeName)
        {
            var child = getChildWithAttribute(src, attributeName);
            return child == null ? null : child.children;
        }

        private static bool doesAttributeExist(jsTreeSaveInputModel src, string attributeName)
        {
            return src.attr["rel"] == attributeName;
        }
    }
}