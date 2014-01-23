# Fun2D

Fun2D is a game-maker that I created for a class of 6th grade students at Jane
Long Middle School as part of after-school program lead by [Citizen
Schools](http://www.citizenschools.org/).  The students used it to learn how to
create a game over a 10-week period in the Fall Semester of 2010.

![screenshot](screenshot.png)

A key feature of Fun2D was its ability on Windows without installation.  During
the 2010 program, each student was given a USB drive containing Fun2D.  This
allowed them to plug the USB drive into any computer (home or school lab) and
run it straight from the USB drive without installation.  The USB Drive also
acted as saved storage for their game they were creating.  (All the students
were given the USB drives after the program ended.)

## Objective

My objective was to teach kids how to make a game by allowing them to
paint characters and scenery that they could put into a simple game.
I wanted them to learn how to move their painted characters around the screen
using simple code, and to transition to a game over screen when their
character hit some falling objects.

Overall, my goal was to teach simple programming concepts in a simple interface
that empowered them to bring their paintings to life.

## How it works

1. Open "Fun2D.exe", making sure that the "common" and "games" folders are in the
same directory.
2. Create a new game, and give it a name.
3. Click "Paint" to create pictures in MSPaint.  Saved images can be seen on the right side of the window.
4. Write code for the game on the left side of the window.  
5. Click "Play" to test your game.

[Watch this video demonstration for more details.](http://www.youtube.com/watch?v=Q2ngpuTfUnQ)

## Tech Details

Fun2D is an editor + game framework.  The editor is a C# .NET WinForms application that
shows a code editor as well as a paint editor (MSPaint).  The game framework
is a heavily simplified layer over [Love2D](https://love2d.org/).  Thus, everything
in the code editor is saved as "game.lua" and is run together with all the other
Lua files in the "common" directory, which are fed into the Love2D executable to run the game.

## Creating an image

You can create and preview images from the editor:

1. Click "Paint" to open MSPaint.
2. Treat the light grey as the transparent color.
3. Notice that the boundary of the image constitutes the full size of the game screen.
4. Resize the image boundary if you want to draw a smaller character.
5. Save the image as some "name".
6. Exit MSPaint.
7. To preview your image in the editor, click the image name on the right panel.
8. To get the coordinates of a position on the image, mouse over the preview image.
9. To copy the coordinates at a desired position, simply click.

## Writing Code

Each function should correspond to a "scene" in the game.  You can click the buttons at the top of the editor to go to that scene's code.
