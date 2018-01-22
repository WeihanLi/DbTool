# DbTool

### BuildStatus

[![Build status](https://ci.appveyor.com/api/projects/status/vo1kipiyqlo8r2kg/branch/master?svg=true)](https://ci.appveyor.com/project/WeihanLi/dbtool/branch/master)

### 简介

这是一个将数据库表生成对应Model的小工具，可以利用这个小工具生成数据库表对应的Model，并且会判断数据表列是否可以为空，可以为空的情况下会使用可空的数据类型，如
int? , DateTime? ，如果数据库中有列描述信息，也会生成在属性名称上添加列描述的注释，支持导出多个表。

**注：不支持外键等需要关联另外一张表的数据关系**

[下载DbTool](https://github.com/WeihanLi/DbTool/releases)

### 为什么使用它

1. [x] 根据表字段信息生成创建表 Sql（Model First）
1. [x] 导入 Excel 文件生成创建表的 Sql（Model First）
1. [x] 根据数据库表信息生成数据库表 Excel 文档（Db First）
1. [x] 根据数据库表信息生成 Model 文件，支持数据列可空导出为可空数据类型/支持导出列描述信息（Db First）
1. [x] 根据 Model 生成 sql 语句（Code First）
1. [x] 支持一次导出多张数据表/支持一次选择多个 Model 文件

### 后续功能

1. [ ] 插件式自定义扩展Model信息

### 功能一览

![DbFirst](resources/desc0.png)

![ModelFirst](resources/desc1.png)

![CodeFirst](resources/desc2.png)

### 使用说明

1. DbFirst

1. ModelFirst

1. CodeFirst