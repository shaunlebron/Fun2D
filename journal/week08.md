# Teaching Week 8

_November 10, 2010_

Today went well. I bought 10 large sheets of paper for brainstorming with the
students. (There is no boardspace available in the classroom.) I started with a
question asking them what week we were in, just to remind them that we are very
close to the end. After a few failed attempts to get the full class’s
attention, I made it clear that if they didn’t pay attention to the important
lesson today, they wouldn’t have a working game. I told them that they will not
hear me if they don’t sit in the front of the class, and that they should move
closer to the front if they really wanted to have a game ready to showcase to
their family and friends. The girls ignored me, and so did the usual problem
students in the back. I started the lesson with only 6 students’ attention, all
in the front.

On one sheet of paper, I wrote down all the code that their game currently had.
I made sure they understood all of it, emphasizing how the computer would read
each instruction in a specific order. I tried to make the program flow clear by
constantly engaging them to answer my progressing questions.

I started a new sheet of paper in a different color marker. I told them I
wanted them to create an enemy, and that I wanted the enemy to start at the top
and slowly drop down, then pop back up when it hit the bottom. The enemy was
created just as the player was, and they seemed to understand that. They still
were having a hard time figuring out how to move the enemy down, but I kept
pointing at the existing code that made the player move, and I started writing
“enemy.y =” and they were able to get the rest : “enemy.y = enemy.y + 10″

To make the enemy wrap back on top, I started them with the template:

```
if _______ then
___________
end
```

I tried to explain to them that this is how the computer makes a decision to
move the enemy back to the top of the screen. The first blank would be the
condition that triggers this decision. I drew a rectangle for the screen and
reminded them that the bottom of the screen is Y=600. I then reminded them that
they can use the “>” or ” 600″. I had to remind them that this could mean “x”
or “y” and that they had to be very specific about which property of the enemy
they were using. And we finally got “enemy.y > 600″.

We then got “enemy.y = 0″ fairly quickly. I did a lot of helping out, and I
can’t expect them to get it outright, but I believe the point of all this is
just to expose them to the style of programming, and get them to start thinking
logically, and I think it will germinate subconsciously. I could tell some
students were grasping it more than others.

During all this, some students kept asking how they were going to kill the
player before we even figured out how to move the enemy. I had to write down
our TO-DO list to let them know we would get to it. So I checked off “how to
move the enemy” and went straight to how to kill the player.

I asked them when would they want the player to die, and they said when it
touches the enemy. I had written a “collide” function that would take care of
this, so I just wrote it out.

```
if _______ then
________
end
```

I told them the first blank contains something new they haven’t seen before,
“collide(player,enemy)”. Granted, they’re not understanding why it works, and I
didn’t think to explain it to them, but it’s just a pixel collision. I then
asked them what they wanted to happen if the player dies, and suggested that
they go to a different screen, a “gameover” screen. I then reminded them of
what the “scene” function does, and one of my students jumped up and spit out
what would go in the 2nd blank, “scene(gameover)”. I gave them a lot of
positive reinforcement. To change a scene, you use the scene function.

So we wrote down our new “gameover” function which just drew the game over
background. So we went to the lab to write it all out. I had my 6 students sit
in the front so I could help them quickly through it. The girls on the second
row asked for my help, and I asked them if they were paying attention today.
They looked embarrassed and said no. I told them that because they weren’t
paying attention, I could not waste time catching them up when I could be
helping the only students who were making the effort to make their game. She
said okay. I know this particular student had talent because when I re-taught
an entire lesson to her table a couple weeks ago, she was the only one who
understood, and was able to answer all my questions. She talked to me at the
end of the class and sincerely asked me to help her catch up next week, and I
told her I would try if she paid attention and sat in the front.

The 6 students were needing help to type out what they had just learned, and
some did better than others. It’s hard to remember something like this, so I
tried to remind them. I was happy to see one of my students finish by the end
of the class. Another student chose to make his enemy move left to right rather
than top to bottom, with wonderful halo pixel art.

One thing I noticed is that the students had a habit of pulling up an internet
flash game while I wasn’t there because they didn’t know what to do next. This
is probably my fault because I am often telling them to hold on while I help
another person. There was less of that this week since I’ve focused on 6
students while basically ignoring the ones who didn’t care. Another thing I
noticed is that one of them would be too anxious with his work, he would just
type a single line of code and hit play without finishing the rest, just to see
his progress.

Unfortunately at this point, the students get a lot of blue error screens. And
I have to debug it for them, because the error messages are relevant to my
framework code, not their game code. Luckily, their code is simple enough that
I can immediately spot what’s wrong, and they continue along fine.
