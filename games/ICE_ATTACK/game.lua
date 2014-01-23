function start()
	background=image("iceattack")
	supergirl =image("cosmogirl")
	enemy = image("spikes")
	enemy.x = 40
	enemy.y = -337
	supergirl.x=0
	supergirl.y=116
	scene(game)
	dead = image("dead")
end

function game()

	draw(background)
	draw(supergirl)
	draw(enemy)
	
	if key("left")then
	supergirl.x=supergirl.x -10
	end
	
	if key("right")then 
	supergirl.x=supergirl.x +10
	end
	
	if supergirl.x < 172 then
		supergirl.y = getY(0,518,172,227,supergirl.x)
	elseif supergirl.x < 227 then
		supergirl.y = getY(172,227,227,380,supergirl.x)
	elseif supergirl.x < 321 then
		supergirl.y = getY(227,380,321,230,supergirl.x)
	elseif supergirl.x < 418 then
		supergirl.y = getY(321,230,421,427,supergirl.x)
	elseif supergirl.x < 503 then
		supergirl.y = getY(421,427,503,260,supergirl.x)
	elseif supergirl.x < 542 then
		supergirl.y = getY(503,260,542,432,supergirl.x)
	elseif supergirl.x < 621 then
		supergirl.y = getY(542,432,621,218,supergirl.x)
	elseif supergirl.x < 781 then
		supergirl.y = getY(621,218,781,436,supergirl.x)
	end
	
	enemy.y = enemy.y + 10
	
	if enemy.y > 600 then
		enemy.y = -337
	end
	
	if collide(supergirl,enemy) then
		scene(gameover)
	end

end

function gameover()
	draw(dead)
	
	if key("enter") then
		supergirl.x = 0
		enemy.y = -337
		scene(game)
	end
end

function getY(x1,y1,x2,y2,x)
	local dx = x2-x1
	local dy = y2-y1
	local m = dy/dx
	return y1 + m*(x-x1) - 138
end