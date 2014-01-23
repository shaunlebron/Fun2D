function start()
background= image("mountains")
halo=image("player")
halo2=image("enemy")
dead=image("gameover")
halo2.x=18
halo2.y=450
halo.x=769
halo.y=436
scene(game)

end

function game()
	draw(background)
	draw(halo)
	draw(halo2)
	if key("left")then
	halo.x=halo.x-10
	end
	if key("right")then 
	halo.x=halo.x+10
	end
 
 	if key("up")then
 	halo.y=halo.y-10
 	end
 	if key("down")then
 	halo.y=halo.y+20
 	end

	halo2.x=halo2.x+10
	if halo2.x > 800 then
	halo2.x=0
	end
	if collide(halo2,halo)then
	scene(gameover)
	end
end

function gameover()
	draw(dead)

	if key("enter")then
	scene(game)
	halo2.x=18
	halo2.y=450
	halo.x=769
	halo.y=436
	end
end
