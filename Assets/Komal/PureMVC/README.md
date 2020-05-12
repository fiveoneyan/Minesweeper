Author: Komal
Date: "2019-07-05"

## PureMVC 改动情况说明
1. Facade 类的构造方法的访问权限改为 private 
2. 所有 GetInstance 更改为 getInstance 
3. Facade 类的 getInstance 方法不传参数，使用默认构造函数
4. Notifier.cs 中获取 Facade 的接口修改
5. 所有类和接口放在 komal.puremvc 命名空间下
6. 去除所有注释
7. 相关成员变量改为小写
8. 增加自定义的类；

