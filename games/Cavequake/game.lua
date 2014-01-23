
function start()
	title("CAVEQUAKE!")
	
	spikes = list(16)
	
	for i=1,#spikes do
		spikes[i] = image("spike")
		spikes[i].x = (i-1)*50
		spikes[i].y = random(-600,-50)
		spikes[i].yspeed = random(2,6)
	end
	
	player = image("player")
	player.x = 400
	player.y = 480
	
	cave = image("cave")
	cave.xspeed = 4
	
	gameover = image("gameover")
	sun = image("sun")
	
	scene(intro)
	fade(1,"white")
	
	fill(false)
	thick(6)
end

----------------------------
function intro()

	draw(cave)
	
	textsize(100)
	textstart(0,200)
	textalign("center")
	color("light blue")
	text("CAVEQUAKE!")
	
	textsize(30)
	text("(a simple game by Mr. Williams)")
	
	textsize(25)
	color("black")
	text("")
	text("Press Enter to start!")
	
	if key("enter") then
		scene(shake)
		fade(2,"black")
		startclock("quake")
	end
	
end

---------------------------
function shake()
	draw(cave)
	player.image = "nervous"
	draw(player)
	
	color("light yellow")
	textstart(0,220)
	textalign("center")
	textsize(80)
	text("EARTHQUAKE!!")
	
	if cave.x > 15 then
		cave.xspeed = -5
	elseif cave.x < -15 then
		cave.xspeed = 5
	end
	
	cave.x = cave.x + cave.xspeed
	
	if clock("quake") > 2.5 then
		scene(game)
		fade(0)
		startclock("survive")
		cave.x = 0
	end
end

------------------------------
function game()

	draw(cave)
	
	if clock("survive") < 2 then
		color("red")
		textstart(0,220)
		textalign("center")
		textsize(80)
		text("LOOKOUT!")
	end
	
	if key("right") then
		player.x = player.x + 3
	end

	if key("left") then
		player.x = player.x - 3
	end
	
	if player.x < 0 then
		player.x = 0
	end
	
	if player.x > 750 then
		player.x = 750
	end
	
	player.image = "player"
	draw(player)
	
	for i=1,#spikes do
		spikes[i].y = spikes[i].y + spikes[i].yspeed

		if spikes[i].y > 600 + 50 then
			spikes[i].y = -50
			spikes[i].yspeed = random(2,5)
		end

		draw(spikes[i])

		if collide(player,spikes[i]) then
			stopclock("survive")
			player.image = "deadplayer"
			draw(player)
			scene(dead)
			fade(5,"white")
		end
	end
end

-----------------------------
function dead()

	background("light grey")
	draw(gameover)
	draw(sun,"light yellow glass")
	
	textsize(90)
	textalign("center")
	textstart(0,20)
	color("purple")
	text("GAME OVER")
	
	textsize(50)
	color("white")
	text("You survived for ")
	text(clock("survive").." seconds.")
	
	textsize(30)
	color("white glass")
	text("")
	text("Press Enter to go back...")
	
	if key("enter") then
		scene(shake)
		fade(2,"black")
		for i=1,#spikes do
			spikes[i].x = (i-1)*50
			spikes[i].y = random(-600,-50)
			spikes[i].yspeed = random(2,5)
		end
		startclock("quake")
		player.image="player"
		player.x = 400
	end
end