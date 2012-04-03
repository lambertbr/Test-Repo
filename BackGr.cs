using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
//Uses Buffers, Vga256(not needed), Palettes

namespace MarioFiles
{
   class BackGr
   {
      //constants
      public const int LEFT = 0;
      public const int RIGHT = 1;
      public const int SHIFT = 16;
      private const int SPEED = 3;
      private const int BRICK_SPEED = 2;
      private const int MAX = (Buffers.MaxWorldSize / SPEED) * Buffers.W;
      private const int HEIGHT = 26;
      private const int CLOUD_SPEED = 4;
      private const int MAX_CLOUDS = 7;
      private const int MIN_CLOUD_SIZE = 30;
      private const int MAX_CLOUD_SIZE = 70;
      private const int CLOUD_HEIGHT = 19;

      //Array Sizes
      private const int COLORMAP_MAX = Buffers.NV * Buffers.H - 1; //might have to do this somewhere else
      //private const int CLOUDMAP_MAP = 

      //variables
      public byte BackGround;
      private byte[] BackGrMap = new byte[MAX];
      private ushort[] ColorMap = new ushort[COLORMAP_MAX];
      /* Still need these private vars
       * CloudMap: array [1 .. 2 * MaxClouds, 0 .. 1] of Integer;
       */
      private byte Clouds;

      //Methods
      public void InitBackGr(byte NewBackGr, byte bClouds)
      {
         int i, j, h;
         //  X, Y, Z: Real;
         //  F: Text;
         BackGround = NewBackGr;
         //NOT FINISHED HERE
      }

      public void DrawBackGr(bool FirstTime)
      {

      }

      public void DrawBackGrMap(int Y1, int Y2, int Shift, byte C)
      {

      }

      public void StartClouds()
      {
         if (Clouds == 0)
         {
            for (int i = Buffers.XView + MAX_CLOUD_SIZE; i >= Buffers.XView; i++)
            {
               Buffers.XView = i;
               PutClouds(i / CLOUD_SPEED, -CLOUD_SPEED);
            }  
         }
      }

      public void DrawPalBackGr()
      {

      }

      public void ReadColorMap()
      {

      }

      public void DrawBricks(int X, int Y, int W, int H)
      {

      }

      public void LargeBricks(int X, int Y, int W, int H)
      {

      }

      public void Pillar(int X, int Y, int W, int H)
      {

      }

      public void Windows(int X, int Y, int W, int H)
      {

      }

      public void DrawBackGrBlock(int X, int Y, int W, int H)
      {

      }

      public void SmoothFill(int X, int Y, int W, int H)
      {

      }

      private void InitClouds()
      {
         int i, j, Tmp0, Tmp1;
      }

      private void TraceCloud(int X, int Y, int N, byte Dir, byte Attr, byte Ovr)
      {
         int min, max;
         byte ok;
      }

      private void PutClouds(int Offset, int N)
      {
         int i, X1, X2, Y;
         byte Attr, Ovr, Size, XSize;
         if (Clouds != 0)
         {
            i = 1;
            while (i <= MAX_CLOUDS)
            {
               Attr = Clouds;
               Ovr = 0xE0;
               X1 = Buffers.XView - Offset + CloudMap[i, 0];
               X2 = Buffers.XView - Offset + CloudMap[i + MAX_CLOUDS, 0];
               XSize = Convert.ToByte(X2 - X1 + 1);
               Y = CloudMap[i, 1];

               if (N > 0)
               {
                  Size = 0;
                  if ((X2 + 10) >= (Buffers.XView + Buffers.NH * Buffers.W))
                     Size = 10;
                  if (((X2 + 10) > Buffers.XView) && (X2 < (Buffers.XView + Buffers.NH * Buffers.W + 10)))
                     TraceCloud(X2 - N - Size, Y, N + Size, RIGHT, Attr, Ovr);
                  if (((X1 + 10) > Buffers.XView) && (X1 < (Buffers.XView + Buffers.NH * Buffers.W)))
                  {
                     TraceCloud(X1 - N, Y, N, LEFT, Ovr, Attr);
                     if (X2 >= (Buffers.XView + Buffers.NH * Buffers.W))
                        TraceCloud(X1, Y, XSize, LEFT, Attr, Ovr);
                  }
               }
               if (N < 0)
               {
                  if (((X2 + 10) > Buffers.XView) && (X2 < (Buffers.XView + Buffers.NH * Buffers.W + 10)))
                  {
                     TraceCloud (X2, Y, - N, RIGHT, Ovr, Attr);
                     if (X1 <= (Buffers.XView - 10))
                        TraceCloud (X2 - XSize, Y, XSize, RIGHT, Attr, Ovr);
                  }
                  Size = 0;
                  if (X1 < (Buffers.XView + 10))
                     Size = 10;
                  if (((X1 + 10) > Buffers.XView) && (X1 < (Buffers.XView + Buffers.NH * Buffers.W + 10)))
                     TraceCloud (X1, Y, - N + Size, LEFT, Attr, Ovr);
               }
               i++;
            }
         }
      }

      private void PutBackGr(bool Map, bool Fill)
      {
         int Y, PageOffset, X1, X2, XPos, X1Pos, X2Pos, DX, OldXView, XStart, OldXStart, Count;
         byte Bank;

         PageOffset = VGA256.GetPageOffset();//Getting rid of VGA file so this won't work
         OldXView = Buffers.LastXView[VGA256.CurrentPage()]; //VGA file use
         Y = PageOffset + (Options.Horizon - HEIGHT) * VGA256.BYTES_PER_LINE; //VGA file
         X1 = Y + Buffers.XView / 4;
         X2 = Y + (Buffers.XView + Buffers.NH * Buffers.W) / 4;
         Bank = Buffers.XView & 3;
         DX = Buffers.XView - OldXView;
         XPos = Buffers.XView;
         X1Pos = Buffers.XView;
         X2Pos = OldXView + Buffers.NH * Buffers.W - 1;
         if (DX < 0)
         {
            X1Pos = OldXView;
            X2Pos = Buffers.XView + Buffers.NH * Buffers.W - 1;
         }
         XStart = Buffers.XView / SPEED;
         OldXStart = OldXView / SPEED + DX;
          //     asm
          //      push    ds
          //      push    es
          //      mov     ax, VGA_SEGMENT
          //      mov     es, ax
          //      lds     si, Map
          //      cld
          //      mov     Count, 4
          //@1:   mov     cl, Bank
          //      mov     ah, 1
          //      shl     ah, cl
          //      mov     al, MAP_MASK
          //      mov     dx, SC_INDEX
          //      out     dx, ax
          //      mov     ah, cl
          //      mov     al, READ_MAP
          //      mov     dx, GC_INDEX
          //      out     dx, ax
          //      mov     dx, XPos
          //      mov     al, $F0
          //      mov     di, X1
          //      mov     cx, OldXStart
          //      mov     bx, XStart
          //@4:   push    bx
          //      push    cx
          //      push    dx
          //      push    di
          //      mov     ah, [bx + si]  { new position }
          //      mov     bx, cx
          //      mov     cl, [bx + si]  { old position }
          //      mov     ch, 0
          //      cmp     Fill, 0
          //      jnz     @Fill
          //      cmp     dx, X1Pos
          //      jb      @Fill
          //      cmp     dx, X2Pos
          //      ja      @Fill
          //      cmp     ah, cl
          //      jz      @5
          //      jl      @8
          //@6:   push    ax
          //      mov     ax, BYTES_PER_LINE
          //      mul     cx
          //      add     di, ax
          //      pop     ax
          //@7:   seges   cmp     [di], al
          //      jnz     @@2
          //      sub     al, $10
          //      seges   mov     [di], al
          //      add     al, $10
          //@@2:  add     di, BYTES_PER_LINE
          //      inc     cl
          //      cmp     cl, ah
          //      jb      @7
          //      jmp     @5
          //@8:   push    ax
          //      mov     bx, BYTES_PER_LINE
          //      mov     al, ah
          //      mov     ah, 0
          //      mul     bx
          //      add     di, ax
          //      pop     ax
          //@9:   sub     al, $10
          //      seges   cmp     [di], al
          //      pushf
          //      add     al, $10
          //      popf
          //      jnz     @@1
          //      seges   mov     [di], al
          //@@1:  add     di, BYTES_PER_LINE
          //      inc     ah
          //      cmp     ah, cl
          //      jb      @9
          //@5:   pop     di
          //      pop     dx
          //      pop     cx
          //      pop     bx
          //      add     bx, 4
          //      add     cx, 4
          //      add     dx, 4
          //      inc     di
          //      cmp     di, X2
          //      jb      @4
          //@2:   inc     Bank
          //      cmp     Bank, 4
          //      jnz     @3
          //      and     Bank, 3
          //      inc     X1
          //      inc     X2
          //@3:   inc     OldXStart
          //      inc     XStart
          //      inc     XPos
          //      dec     Count
          //      jnz     @1
          //      pop     es
          //      pop     ds
          //      jmp     @Exit

          //@Fill:
          //      push    bx
          //      push    cx
          //      mov     cl, ch
          //      mov     ch, 0
          //      mov     bl, ah
          //      mov     bh, 0
          //@@5:  cmp     cx, HEIGHT
          //      ja      @@3
          //      cmp     cx, bx
          //      jb      @@4
          //      sub     al, $10
          //      seges   cmp     [di], al
          //      pushf
          //      add     al, $10
          //      popf
          //      jnz     @@7
          //      seges   mov     [di], al
          //@@7:  add     di, BYTES_PER_LINE
          //      inc     cx
          //      jmp     @@5
          //@@4:  seges   cmp     [di], al
          //      jnz     @@6
          //      sub     al, $10
          //      seges   mov     [di], al
          //      add     al, $10
          //@@6:  add     di, BYTES_PER_LINE
          //      inc     cx
          //      jmp     @@5
          //@@3:  pop     cx
          //      pop     bx
          //      jmp     @5

          //@Exit:
      }

      private void BrickPalette(int i)
      {
         i = i % 20;
         for (int j = 0; j <= 19; j++)
         {
            if (i == j)
               Palettes.CopyPalette(0xFE, 0xE0 + j);
            else if (((i + 2) % 20) == j)
               Palettes.CopyPalette(0xFF, 0xE0 + j);
            else
               Palettes.CopyPalette(0xFD, 0xE0 + j);
         }
      }

      private void LargeBrickPalette(int i)
      {
         i = i % 32;
         for (int j = 0; j <= 31; j++)
         {
            if ((i == j) || (((i + 1) % 32) == j))
               Palettes.CopyPalette (0xD6, 0xE0 + j);
            else if ((((i + 3) % 32) == j) || (((i + 4) % 32) == j))
               Palettes.CopyPalette (0xD4, 0xE0 + j);
            else
               Palettes.CopyPalette (0xD1, 0xE0 + j);
         }
      }

      private void PillarPalette(int i)
      {
         const int ShadowPos = 28;
         const int ShadowEnd = 36;
         int j, k, l;
         byte c1, c2, c3, Base;
         Base = Options.BackGrColor1; //Haven't found where options comes from
         c1 = Palette [Base, 0] / 4; //Palette is a Unit used for manipulating any color in 256 color video mode
         c2 = Palette [Base, 1] / 4; //Going to need to use something to replace palette.
         c3 = Palette [Base, 2] / 4;
         i = i % 60;
         j = 0;
         k = 1;
         while( k < 15)
         {
           for (l = j; l <= k; l++)
           {
              Palettes.OutPalette (0xC0 + ((l + i) % 60), c1 + k, c2 + k, c3 + k);
              Palettes.OutPalette (0xC0 + ((ShadowPos + i - l) % 60), c1 + k, c2 + k, c3 + k);
           }
           j = k;
           k = k + 1;
         }
         for(j = ShadowPos; j <= ShadowEnd; j++)
         {
            if (c1 > 0)
                c1--;
            if (c2 > 0)
                c2--;
            if (c3 > 0)
                c3--;
            Palettes.OutPalette (0xC0 + ((j + i) % 60), c1, c2, c3);
         }
         Base = Options.BackGrColor2;
         c1 = Palette [Base, 0] / 4;
         c2 = Palette [Base, 1] / 4;
         c3 = Palette [Base, 2] / 4;
         for( j = ShadowEnd + 1; j <= 59; j++)
              Palettes.OutPalette (0xC0 + ((i + j) % 60), c1, c2, c3);
      }

      private void WindowPalette(int i)
      {
       int j;
       i = i % 32;
       for (j = 0; j <= 5; j++)
          Palettes.CopyPalette (1, 0xE0 + ((i + j) % 32));
       for (j = 6; j <= 31; j++)
          Palettes.CopyPalette (16, 0xE0 + ((i + j) % 32));
      }
   }
}