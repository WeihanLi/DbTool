# DbTool
这是一个将数据库表生成对应Model的小工具，使用说明如下：

1. 设置自己的连接字符串

    ![设置连接字符串](resources/connect.png)

2. 连接数据库
    
    数据库连接成功之后会在窗体左下角，显示 “数据库连接成功”
    
    ![设置连接字符串](resources/connect1.png)

3. 选择要导出 Model 的数据库表，并设置要导出的 Model 的命名空间和前缀与后缀

    Model 和 文件的默认名称是表名称，以“tab”或“tbl”或“tbl_”为前缀的表会先去掉表前缀再取表名称
    默认命名空间是Models（不可为空），默认后缀是 Model （可以为空）
    
    ![设置 Model信息](resources/generateModel.png)

4. 生成Model

    选择要导出的表并设置Model相关信息后，点击 "生成Model" 按钮，点击按钮之后会弹出一个窗口让你选择保存 Model 文件的目录
    
    ![设置 Model信息](resources/chooseDir.png)

    选择之后会将生成的文件保存到选择的文件夹，并自动打开资源管理器到所选目录
    
    ![查看生成 model](resources/generateModel1.png)