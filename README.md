# LightGL - A Lightweight OpenGL C# Wrapper Library

LightGL is a **lightweight C# wrapper library for OpenGL**, offering a concise, efficient, and cross-platform graphics rendering solution.

LightGL是一个针对**OpenGL的C#轻量封装库**，提供简洁、高效、跨平台的图形渲染解决方案。

<details>
<summary><h3> 🌐 中文版说明</h3></summary>
  
## 核心特性

### 🚀 轻量无依赖
- 纯C#实现的OpenGL封装，**无任何第三方依赖**，仅依赖系统原生OpenGL驱动和.NET运行时
- 精简代码结构，核心逻辑聚焦渲染本质，易于理解和扩展
- 生成库文件体积小巧，部署便捷，无额外依赖负担

### 🌍 多平台支持
- 统一接口设计，适配Windows、Linux、macOS等主流操作系统
- 基于平台抽象层实现OpenGL上下文管理，无需修改代码即可跨平台运行
- 兼容OpenGL 3.2+核心配置文件，适配主流显卡驱动
- 统一的 IGlContext 接口为不同操作系统提供了专门的实现：
  - Windows 平台：使用 WGL 实现，支持 OpenGL 3.2 +
  - Linux 平台：使用 GLX 实现，支持 X11 窗口系统，提供了完整的 Linux 平台 OpenGL 上下文管理
  - macOS 平台：使用 Cocoa 的 NSOpenGLContext 实现，支持 macOS 的原生 OpenGL 框架
  - Android 平台：提供了基本的 AndroidGLContext 实现，基于 EGL（Embedded System Graphics Library）接口

### 📦 完整功能封装
- 核心功能覆盖：着色器管理、缓冲区对象、纹理处理、矩阵/向量运算等
- 提供简洁API，降低OpenGL原生接口的使用复杂度
- 包含可直接运行的Demo项目，演示完整渲染流程

### 🎯 实际项目验证
- 已在真实项目中落地使用:
  - PS1模拟器 ScePSX https://github.com/unknowall/ScePSX
  - PSP模拟器 ScePSP https://github.com/unknowall/ScePSP
- Demo项目提供完整示例代码，快速上手无门槛

## 环境要求

- **开发环境**：Visual Studio 2022+ 或支持.NET 6.0+的IDE
- **运行环境**：.NET 6.0+ 运行时
- **硬件要求**：支持OpenGL 3.2+的显卡及对应驱动

## 快速开始

### 1. 获取项目
```bash
git clone https://github.com/unknowall/LightGL.git
```

### 2. 编译项目
- 打开解决方案 `LightGL.sln`
- 选择目标平台（Windows/Linux/macOS）
- 编译生成

### 3. 集成使用
在你的C#项目中引用 `LightGL.dll`，参考以下示例代码快速实现基础渲染：

```csharp
// 1. 创建OpenGL上下文（以窗口句柄为例）
IntPtr windowHandle = YourWindow.GetHandle(); // 替换为你的窗口句柄
IGlContext context = GlContextFactory.CreateFromWindowHandle(windowHandle, 3, 2, GlProfile.Core);
context.MakeCurrent();

// 2. 初始化着色器
var vertexShaderCode = @"
attribute vec4 position;
attribute vec2 texCoord;
varying vec2 v_texCoord;
void main() {
    gl_Position = position;
    v_texCoord = texCoord;
}";

var fragmentShaderCode = @"
uniform sampler2D u_texture;
varying vec2 v_texCoord;
void main() {
    gl_FragColor = texture2D(u_texture, v_texCoord);
}";

GLShader shader = new GLShader(vertexShaderCode, fragmentShaderCode);

// 3. 创建顶点缓冲区
float[] vertices = {
    -1.0f, -1.0f, 0.0f, 0.0f, // 左下
     1.0f, -1.0f, 1.0f, 0.0f, // 右下
     1.0f,  1.0f, 1.0f, 1.0f, // 右上
    -1.0f,  1.0f, 0.0f, 1.0f  // 左上
};
GLBuffer vertexBuffer = GLBuffer.Create();
vertexBuffer.SetData(vertices);

// 4. 创建纹理
GLTexture2D texture = GLTexture2D.Create()
    .SetFormat(TextureFormat.RGBA)
    .SetSize(2, 2)
    .SetData(new uint[] { 0xFF0000FF, 0xFF00FFFF, 0xFFFF00FF, 0xFFFFFFFF });

// 5. 渲染循环
bool running = true;
while (running)
{
    // 清除缓冲区
    GL.ClearColor(0.1f, 0.1f, 0.1f, 1.0f);
    GL.Clear(GL.GL_COLOR_BUFFER_BIT);
    
    // 绑定资源并绘制
    shader.Use();
    shader.SetUniform("u_texture", texture);
    vertexBuffer.Bind();
    GL.DrawArrays(GL.GL_TRIANGLE_FAN, 0, 4);
    
    // 交换缓冲区
    context.SwapBuffers();
    
    // 处理窗口事件（根据你的窗口框架实现）
    HandleWindowEvents(ref running);
}

// 资源释放
shader.Dispose();
vertexBuffer.Dispose();
texture.Dispose();
context.Dispose();
```

## 项目结构

```
LightGL/
├── Demo/                # 演示项目（完整可运行）
├── GLControl/           # 可视化渲染控件（Windows Forms控件）
├── LightGL/             # 核心库源码
│   ├── Platform/        # 平台适配层（Windows/Linux/macOS）
│   ├── Utils/           # 工具类（矩阵、向量、纹理等）
│   ├── GL.cs            # OpenGL核心封装
│   ├── IGlContext.cs    # 跨平台上下文接口
│   └── GlContextFactory.cs # 上下文创建工厂
├── LightGL.sln          # 解决方案文件
├── LICENSE.txt          # 开源许可证
└── README.md            # 项目说明文档
```

## 运行Demo

1. 编译解决方案后，启动 `Demo` 项目
2. Demo将展示：窗口创建、上下文初始化、着色器编译、纹理渲染等核心流程
3. 可直接参考Demo源码，快速迁移到你的项目中

## 贡献指南
1. **提交Issue**：报告Bug、提出功能建议或疑问
2. **提交PR**：
   - Fork本仓库
   - 创建特性分支（`feature/xxx`）或修复分支（`fix/xxx`）
   - 提交代码并确保编译通过
   - 发起Pull Request，描述修改内容
3. 代码风格：遵循C#编码规范，保持代码简洁可读

## 许可证

本项目采用 **MIT License** 开源协议，允许：
- 商业使用、修改、分发
- 私人使用
- 无需公开修改后的源码（但需保留原版权声明）

------
</details>

## Core Features

### 🚀 Lightweight & Dependency-Free
- Pure C# implementation of OpenGL wrapper with **zero third-party dependencies**, relying solely on system-native OpenGL drivers and the .NET runtime
- Streamlined code structure with core logic focused on the essence of rendering, facilitating easy understanding and extension
- Compact generated library size for hassle-free deployment with no extra dependency burden

### 🌍 Cross-Platform Support
- Unified interface design compatible with mainstream operating systems including Windows, Linux, and macOS
- OpenGL context management implemented via a platform abstraction layer, enabling cross-platform operation without code modification
- Compatible with OpenGL 3.2+ core profile and mainstream graphics card drivers
- The unified `IGlContext` interface provides dedicated implementations for different operating systems:
  - **Windows**: Built on WGL, supporting OpenGL 3.2+ core profile
  - **Linux**: Built on GLX, supporting the X11 window system with complete OpenGL context management for Linux platforms
  - **macOS**: Built on Cocoa’s `NSOpenGLContext`, supporting macOS’s native OpenGL framework
  - **Android**: Provides a basic `AndroidGLContext` implementation based on the EGL (Embedded System Graphics Library) interface

### 📦 Comprehensive Feature Wrapper
- Core functionality coverage: shader management, buffer objects, texture processing, matrix/vector operations, etc.
- Offers a simplified API that reduces the complexity of using native OpenGL interfaces
- Includes out-of-the-box demo projects demonstrating complete rendering workflows

### 🎯 Battle-Tested in Real-World Projects
- Proven in production-grade applications:
  - PS1 Emulator ScePSX: https://github.com/unknowall/ScePSX
  - PSP Emulator ScePSP: https://github.com/unknowall/ScePSP
- Demo projects provide complete sample code for quick and barrier-free onboarding

## Environment Requirements
- **Development Environment**: Visual Studio 2022+ or any IDE supporting .NET 6.0+
- **Runtime Environment**: .NET 6.0+ runtime
- **Hardware Requirements**: Graphics card supporting OpenGL 3.2+ with corresponding drivers installed

## Quick Start

### 1. Get the Project
```bash
git clone https://github.com/unknowall/LightGL.git
```

### 2. Build the Project
- Open the solution `LightGL.sln`
- Select the target platform (Windows/Linux/macOS)
- Build the project

### 3. Integration & Usage
Reference `LightGL.dll` in your C# project, and use the following sample code to quickly implement basic rendering:

```csharp
// 1. Create OpenGL context (window handle example)
IntPtr windowHandle = YourWindow.GetHandle(); // Replace with your actual window handle
IGlContext context = GlContextFactory.CreateFromWindowHandle(windowHandle, 3, 2, GlProfile.Core);
context.MakeCurrent();

// 2. Initialize shaders
var vertexShaderCode = @"
attribute vec4 position;
attribute vec2 texCoord;
varying vec2 v_texCoord;
void main() {
    gl_Position = position;
    v_texCoord = texCoord;
}";

var fragmentShaderCode = @"
uniform sampler2D u_texture;
varying vec2 v_texCoord;
void main() {
    gl_FragColor = texture2D(u_texture, v_texCoord);
}";

GLShader shader = new GLShader(vertexShaderCode, fragmentShaderCode);

// 3. Create vertex buffer
float[] vertices = {
    -1.0f, -1.0f, 0.0f, 0.0f, // Bottom-left
     1.0f, -1.0f, 1.0f, 0.0f, // Bottom-right
     1.0f,  1.0f, 1.0f, 1.0f, // Top-right
    -1.0f,  1.0f, 0.0f, 1.0f  // Top-left
};
GLBuffer vertexBuffer = GLBuffer.Create();
vertexBuffer.SetData(vertices);

// 4. Create texture
GLTexture2D texture = GLTexture2D.Create()
    .SetFormat(TextureFormat.RGBA)
    .SetSize(2, 2)
    .SetData(new uint[] { 0xFF0000FF, 0xFF00FFFF, 0xFFFF00FF, 0xFFFFFFFF });

// 5. Render loop
bool running = true;
while (running)
{
    // Clear buffers
    GL.ClearColor(0.1f, 0.1f, 0.1f, 1.0f);
    GL.Clear(GL.GL_COLOR_BUFFER_BIT);
    
    // Bind resources and draw
    shader.Use();
    shader.SetUniform("u_texture", texture);
    vertexBuffer.Bind();
    GL.DrawArrays(GL.GL_TRIANGLE_FAN, 0, 4);
    
    // Swap buffers
    context.SwapBuffers();
    
    // Handle window events (implement based on your windowing framework)
    HandleWindowEvents(ref running);
}

// Release resources
shader.Dispose();
vertexBuffer.Dispose();
texture.Dispose();
context.Dispose();
```

## Project Structure
```
LightGL/
├── Demo/                # Demo projects (fully runnable)
├── GLControl/           # Visual rendering control (Windows Forms control)
├── LightGL/             # Core library source code
│   ├── Platform/        # Platform abstraction layer (Windows/Linux/macOS)
│   ├── Utils/           # Utility classes (matrix, vector, texture, etc.)
│   ├── GL.cs            # Core OpenGL wrapper
│   ├── IGlContext.cs    # Cross-platform context interface
│   └── GlContextFactory.cs # Context creation factory
├── LightGL.sln          # Solution file
├── LICENSE.txt          # Open source license
└── README.md            # Project documentation
```

## Run the Demo
1. After building the solution, launch the `Demo` project
2. The demo showcases core workflows: window creation, context initialization, shader compilation, texture rendering, etc.
3. Demo source code can be directly referenced for quick migration to your own projects

## Contribution Guidelines
1. **Submit Issues**: Report bugs, suggest features, or ask questions
2. **Submit Pull Requests (PRs)**:
   - Fork this repository
   - Create a feature branch (`feature/xxx`) or a bugfix branch (`fix/xxx`)
   - Commit your code and ensure successful compilation
   - Open a Pull Request with a clear description of your changes
3. **Code Style**: Follow C# coding standards to maintain concise and readable code

## License
This project is licensed under the **MIT License**, which permits:
- Commercial use, modification, and distribution
- Private use
- No requirement to disclose modified source code (original copyright notice must be retained)

Full license text is available in [LICENSE.txt](LICENSE.txt)

## Contact & Feedback
- For questions or suggestions, feel free to submit Issues or Pull Requests

## 联系与反馈

- 如有问题或建议，欢迎提交Issue或Pull Request
