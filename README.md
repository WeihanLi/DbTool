# DbTool

一个支持 DbFirst、ModelFirst 和 CodeFirst 的数据库工具。

[![Build Status](https://weihanli.visualstudio.com/Pipelines/_apis/build/status/WeihanLi.DbTool?branchName=wpf-dev)](https://weihanli.visualstudio.com/Pipelines/_build/latest?definitionId=18&branchName=wpf-dev)

[![GitHub release](https://img.shields.io/github/release/WeihanLi/DbTool.svg?style=plastic)](https://github.com/WeihanLi/DbTool/releases/latest)

## 简介

这是一个针对 `SqlServer` 和 `C#` 的数据库的小工具，可以利用这个小工具生成数据库表对应的 Model，并且会判断数据表列是否可以为空，可以为空的情况下会使用可空的数据类型，如
int? , DateTime? ，如果数据库中有列描述信息，也会生成在属性名称上添加列描述的注释，支持导出多个表；可以导出到Excel，可以根据Excel字段文档生成Sql，数据库表误删除又没有备份的时候就很有帮助了，而且支持反向的根据生成的Model去生成创建数据库表的Sql。

## Packages

- `DbTool.Core` DbTool 用于扩展的接口定义，帮助类
- `DbTool.DbProvider.MySql` DbTool 对于 MySql 的支持
- `DbTool.DbProvider.SqlServer` DbTool 对于 SqlServer 的支持

## 扩展

1. 扩展数据库支持，实现 `IDbProvider`
1. 扩展导出方式，实现 `IDbDocExporter`
1. 扩展 Model 代码生成方式，实现 `IModelCodeGenerator`
1. 扩展 Model 名称表名称转化，实现 `IModelNameConverter`，也可以继承 `DefaultModelNameConverter`，改写某一个实现

