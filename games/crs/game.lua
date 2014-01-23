 function start()
    background = image ("background") 
    player = image ("player")
    player.x = 364
    player.y = 536
    scene(game)
    missle = image("missle")
    missle.x = 433
    missle.y = -20
    dead = image("dead")
end



function game()
       draw(background)
       draw(player)
       draw(missle)
    
    if key ("left")then
       player.x = player.x -10
       
    end
       
    if key ("right")then
       player.x = player.x +10
    end
       
    missle.y=missle.y+10
    
    if missle.y > 600 then
    	missle.y = -10
    end
    
    if collide (missle,player)then
    scene(gameover)
    end
    
end
       
function gameover()
	draw(dead)
end