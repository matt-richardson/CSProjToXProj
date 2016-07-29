﻿using System;
using CSProjToXProj;
using CSProjToXProj.SourceFiles;
using FluentAssertions;
using NUnit.Framework;
using Tests.Plumbing;

namespace Tests
{
    public class WriterTests
    {
        [Test]
        public void XProjIsWrittenCorrectly()
        {
            var fs = new FakeFileSystem();
            var metadata = new ProjectMetadata("v4.5.1", "MyProject.Namespace", Guid.Parse("50da3bcc-0fbb-4b69-8c7a-077f01fd6e4e"), new []{ "MyLib" });
            new Writer(fs).WriteXProj(@"x:\path\myproj.csproj", metadata);

            fs[@"x:\path\myproj.xproj"].Should().Be(@"<?xml version=""1.0"" encoding=""utf-8""?>
<Project ToolsVersion=""14.0"" DefaultTargets=""Build"" xmlns=""http://schemas.microsoft.com/developer/msbuild/2003"">
  <PropertyGroup>
    <VisualStudioVersion Condition=""'$(VisualStudioVersion)' == ''"">14.0</VisualStudioVersion>
    <VSToolsPath Condition=""'$(VSToolsPath)' == ''"">$(MSBuildExtensionsPath32)\Microsoft\VisualStudio\v$(VisualStudioVersion)</VSToolsPath>
  </PropertyGroup>
  <Import Project=""$(VSToolsPath)\DotNet\Microsoft.DotNet.Props"" Condition=""'$(VSToolsPath)' != ''"" />
  <PropertyGroup Label=""Globals"">
    <ProjectGuid>50da3bcc-0fbb-4b69-8c7a-077f01fd6e4e</ProjectGuid>
    <RootNamespace>MyProject.Namespace</RootNamespace>
    <BaseIntermediateOutputPath Condition=""'$(BaseIntermediateOutputPath)'=='' "">.\obj</BaseIntermediateOutputPath>
    <OutputPath Condition=""'$(OutputPath)'=='' "">.\bin\</OutputPath>
  </PropertyGroup>
  <PropertyGroup>
    <SchemaVersion>2.0</SchemaVersion>
  </PropertyGroup>
  <Import Project=""$(VSToolsPath)\DotNet\Microsoft.DotNet.targets"" Condition=""'$(VSToolsPath)' != ''"" />
</Project>");
        }

        [Test]
        public void ProjectJsonIsWrittenCorrectly()
        {
            var fs = new FakeFileSystem();
            var packages = new[] { new PackageEntry("Foo", "3.4.1"), new PackageEntry("Bar", "0.0.1-alpha.2"), };
            var metadata = new ProjectMetadata("v4.5.1", "MyProject.Namespace", Guid.Parse("50da3bcc-0fbb-4b69-8c7a-077f01fd6e4e"), new[] { "MyLib" });
            new Writer(fs).WriteProjectJson(@"x:\path", metadata, packages);

            fs[@"x:\path\project.json"].Should().Be(@"{
  ""version"": ""1.0.0-*"",

  ""dependencies"": {
    ""Foo"": ""3.4.1"",
    ""Bar"": ""0.0.1-alpha.2"",
    ""MyLib"": ""*""
  },

  ""frameworks"": {
    ""net451"": {
    }
  },
}");
        }
    }
}