# SoftwareRenderer
个人为学习图形学和熟悉固定管线流程,使用C#写的软光栅器，未考虑任何性能优化。在较为陌生的知识点上都标注了参考文献的链接。

采用的是左手坐标系，Model/View/Perspective Projection矩阵。工程有VertexShader和FragmentShader两个类，但并不具备真正的Shader功能：
* VertexShader仅完成顶点变换。
* FragmentShader仅完成颜色拷贝（如果以后实现了光源的话，这里可能会尝试实现不同的光照方程）。

目前暂未实现：
1、 裁剪
2、 正交投影
3、 纹理
4、 法线变换

效果
![RENDER_WIREFRAME模式](https://github.com/xieheng/SoftwareRenderer/blob/master/image_render_wireframe.png)

![RENDER_COLOR模式](https://github.com/xieheng/SoftwareRenderer/blob/master/image_render_color.png)

方向键左/右：旋转物体。
方向键上/下：拉动相机。
数字键1：切换到线框模式。
数字键2：切换到颜色模式。

最重要的
本人目前处在图形学的起步阶段，代码中肯定有各种错误（尤其是对知识点理解的错误），如果您发现了请一定给我发邮件不吝赐教：xieheng84@163.com，拜谢！
