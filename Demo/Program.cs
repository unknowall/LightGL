using SDL2;
using System;
using System.Diagnostics;
using System.Windows.Forms;

using LightGL;

#pragma warning disable CS8618
#pragma warning disable CS8625

class Program
{
    static IntPtr window;
    static IGlContext Context;
    static GLShader Shader;
    static GLBuffer VertexBuffer;
    static GLBuffer TexCoordsBuffer;
    static GLTexture2D DrawTexture;
	//static GLTexture2D DrawDepth;

    public class ShaderInfoClass
    {
        public GlAttribute position;
        public GlAttribute texCoords;
        public GlUniform texture;
    }
    static ShaderInfoClass ShaderInfo = new ShaderInfoClass();

    static RectangleF TextureRect;

    [STAThreadAttribute]
    private static void Main(string[] args)
    {
        if (SDL.SDL_Init(SDL.SDL_INIT_VIDEO | SDL.SDL_INIT_AUDIO) != 0)
        {
            Console.Error.WriteLine("Couldn't initialize SDL");
            return;
        }

        window = SDL.SDL_CreateWindow(
            "LightGT",
            SDL.SDL_WINDOWPOS_CENTERED, SDL.SDL_WINDOWPOS_CENTERED,
            1024, 768,
            SDL.SDL_WindowFlags.SDL_WINDOW_SHOWN | SDL.SDL_WindowFlags.SDL_WINDOW_RESIZABLE
        );

        SDL.SDL_SysWMinfo wmInfo = new SDL.SDL_SysWMinfo();
        SDL.SDL_VERSION(out wmInfo.version);
        SDL.SDL_GetWindowWMInfo(window, ref wmInfo);
        IntPtr windowhwnd = wmInfo.info.win.window;

        Context = GlContextFactory.CreateFromWindowHandle(windowhwnd);
        Context.MakeCurrent();

		VertexBuffer = GLBuffer.Create().SetData(RectangleF.FromCoords(-1, -1, +1, +1).GetFloat2TriangleStripCoords());

        Shader = new GLShader(
            "attribute vec4 position; attribute vec4 texCoords; varying vec2 v_texCoord; void main() { gl_Position = position; v_texCoord = texCoords.xy; }",
            "uniform sampler2D texture; varying vec2 v_texCoord; void main() { gl_FragColor = texture2D(texture, v_texCoord); }"
        );
        
        Shader.BindUniformsAndAttributes(ShaderInfo);

		TextureRect = RectangleF.FromCoords(0, 0, 1024 / 1024f, 768 / 768f);
		TextureRect.VFlip();
		TexCoordsBuffer = GLBuffer.Create().SetData(TextureRect.GetFloat2TriangleStripCoords());
		ShaderInfo.position.SetData<float>(VertexBuffer, 2);
		ShaderInfo.texCoords.SetData<float>(TexCoordsBuffer, 2);

        DrawTexture = GLTexture2D.Create().SetFormat(TextureFormat.RGBA).SetSize(2, 2).SetData(new uint[] { 0xFF0000FF, 0xFF00FFFF, 0xFFFF00FF, 0xFFFFFFFF });

        GL.Viewport(0, 0, 1024, 768);
        GL.ClearColor(0, 0, 0, 1);
        GL.Clear(GL.GL_COLOR_BUFFER_BIT);

        Shader.Draw(GLGeometry.GL_TRIANGLE_STRIP, 4, () =>
        {
            ShaderInfo.texture.Set(GLTextureUnit.CreateAtIndex(0).SetFiltering(GLScaleFilter.Nearest).SetWrap(GLWrap.ClampToEdge).SetTexture(DrawTexture));
            //ShaderInfo.texture.Set(GLTextureUnit.CreateAtIndex(0).SetFiltering(GLScaleFilter.Nearest).SetWrap(GLWrap.ClampToEdge).SetTexture(DrawDepth));
        });

        Context.SwapBuffers();

		MainLoop();
    }

    private static void MainLoop()
    {
        bool running = true;

        while (running)
        {
            while (SDL.SDL_PollEvent(out var e) != 0)
            {
                switch (e.type)
                {
                    case SDL.SDL_EventType.SDL_QUIT:
                        running = false;
                        break;
                    case SDL.SDL_EventType.SDL_KEYDOWN:
                    case SDL.SDL_EventType.SDL_KEYUP:
                        var pressed = e.type == SDL.SDL_EventType.SDL_KEYDOWN;
                        switch (e.key.keysym.sym)
                        {
                            case SDL.SDL_Keycode.SDLK_KP_ENTER:
                                running = false;
                                break;
                            default:
                                break;
                        }
                        ;

                        break;
                }
            }
        }

    }

}