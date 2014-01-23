# Teaching Week 6

_October 27, 2010_

This week was a bit hard. I printed out a 10 page worksheet that taught
conditions, key presses, and relative movement for the player. The idea was to
get the player to move in a certain direction depending on the key being
pressed.

I started with conditions. A condition is what the computer uses to make a
decision. They filled out the blanks of each page, but when we started using
what they filled in for the blanks of the previous pages, they were not able to
recall them. Building on previous knowledge was not easy because I would go on
without them truly understanding what they were doing. It doesn’t help that I’m
constantly losing their attention when they turn to talk to their friends, and
they immediately lose interest.

I had to go back when they couldn’t write down the code for moving a player left:

```
if key(“left”) then
	player.x = player.x – 10
end
```

They kept trying to write down “player.x = -10″. After a few failed attempts to
explain why that would place the player at x = -10, I believe I finally got
them to understand it. I had to draw a table showing player.x over time, and
how “player.x = -10″ would never move the player left. I told them that they
had to take the current value and subtract 10 from it, and then store it back
to player.x.

After I had finally gotten the kids in the front to understand it – at least
those paying attention – I went to the students in the back, and they had
nothing written down for the problem that I had been teaching. I had even
called for participation of the students, kept asking them questions, and had
them write answers on the board for all to follow. They said they couldn’t hear
me. So while all the other kids were impatient and ready to go to the lab, I
had to have them sit down and wait while I repeated my entire lecture for the
students who fell behind. Only one student out of the four understood it. The
others were writing notes, laughing, and generally showing disinterest.

Next week, I plan on making a quick review discussing movement, then telling them to implement in the lab. The only thing they need to do after that is:

1. Keep the player on the screen
2. Create projectiles
3. Move Projectiles at random speeds
4. Reset Projectile position when falls under screen
5. Game over screen when player hits projectile
6. General scene transition (intro, game, gameover)

I will have to allocate these for week 8 and 9.
