# LightGL - A Lightweight OpenGL C# Wrapper Library

LightGL is a **lightweight C# wrapper library for OpenGL**, offering a concise, efficient, and cross-platform graphics rendering solution.

LightGL is built specifically for high-performance graphics rendering. 
Its call efficiency is almost identical to that of native C/C++ implementations, outperforming general-purpose OpenGL wrapper libraries.

**file size: LightGL.1.0.2.nupkg - 54kb**

## Core Features

### Lightweight & Dependency-Free
- Pure C# implementation of OpenGL wrapper with **zero third-party dependencies**, relying solely on system-native OpenGL drivers and the .NET runtime
- Streamlined code structure with core logic focused on the essence of rendering, facilitating easy understanding and extension
- Compact generated library size for hassle-free deployment with no extra dependency burden

### Cross-Platform Support
- Unified interface design compatible with mainstream operating systems including Windows, Linux, and macOS
- OpenGL context management implemented via a platform abstraction layer, enabling cross-platform operation without code modification
- Compatible with OpenGL 3.2+ and mainstream graphics card drivers
- The unified `IGlContext` interface provides dedicated implementations for different operating systems:
  - **Windows**: Built on WGL, supporting OpenGL 3.2+
  - **Linux**: Built on GLX, supporting the X11 window system with complete OpenGL context management for Linux platforms
  - **macOS**: Built on Cocoa’s `NSOpenGLContext`, supporting macOS’s native OpenGL framework
  - **Android**: Provides a basic `AndroidGLContext` implementation based on the EGL (Embedded System Graphics Library) interface

### Comprehensive Feature Wrapper
- Core functionality coverage: shader management, buffer objects, texture processing, matrix/vector operations, etc.
- Offers a simplified API that reduces the complexity of using native OpenGL interfaces
- Includes out-of-the-box demo projects demonstrating complete rendering workflows

### Battle-Tested in Real-World Projects
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
Reference the LightGL.dll in your C# project, or install the LightGL package via NuGet. Refer to the following sample code to quickly implement basic rendering.:

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

## Contact & Feedback
- For questions or suggestions, feel free to submit Issues or Pull Requests

