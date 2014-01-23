 function start()
   	backround = image("backround")
	player = image("player")
	player.x = 296
	player.y = 403
    missile = image("missile")
    missile.y = 0
    missile.x = 354
    missile2 = image("missile")
    missile2.y = 0
    missile2.x = 187
    missile3 = image("missile")
    missile3.y = 0
    missile3.x = 281
    missile4 = image("missile")
    missile4.y = 0
    missile4.x = 681
    missile5 = image("missile")
    missile5.y = 0
    missile5.x = 96
    scene(game)
	dead = image("dead")
end

function game()
	draw(backround)
	draw(player)
	draw(missile)
    if key("right")then
    player.x = player.x +10
   end
    if key("left")then
    player.x = player.x -10
    end
    missile.y = missile.y +10
    missile2.y = missile2.y +15
    missile3.y = missile3.y +20
    missile4.y = missile4.y +25
    missile5.y = missile5.y +30	
    if missile.y > 600 then
    missile.y = -230
    end
    if missile2.y > 600 then
    missile.y = -230
    end
    if missile3.y > 600 then
    missile3.y = -230
    end
    if missile4.y > 600 then
    missile4.y = -230
    end
        if missile5.y > 600 then
    missile5.y = -230
    end
    
    draw(missile)
    draw(missile2)
    draw(missile3)
    draw(missile4)
    draw(missile5)
    if collide(missile,player) then
    scene(gameover)
    end
     if collide(missile2,player) then
    scene(gameover)
    end
     if collide(missile3,player) then
    scene(gameover)
    end
     if collide(missile4,player) then
    scene(gameover)
    end
     if collide(missile5,player) then
    scene(gameover)
    end
    
end

function gameover()
    draw(dead)
    if key("enter") then
    scene(game)
    end
    player.x = 296
    player.y = 403
    missile.y = 0
    missile.x = 354
    if key("enter") then
    missile.y = 0
    missile.x = 354 
      missile2.y = 0
    missile2.x = 187
      missile3.y = 0
    missile3.x = 281
      missile4.y = 0
    missile4.x = 681
      missile5.y = 0
    missile5.x = 96
    end
end