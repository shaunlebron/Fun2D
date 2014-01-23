 function start()
    background = image("background")
    player = image("player")
    death = image("death")
    player.x = 440
    player.y = 478
    scene(game)
   missile=image("missile")   
        
   missile.y=0
   missile.x=354
    scene(game)
end
    
function game()  
    draw(background)
    draw(player)   
    draw(missile)
    
	if key("left" ) then
		player.x= player.x - 10   
	end
		
	if key("right") then   
 		player.x= player.x+ 10  
 	end
   
      
    if key("up") then
        player.y= player.y-10
    end
   
    if key("down")then
         player.y= player.y+10
    end     
    
    if player.x<0 then
   	 player.x=0
   end
   
   missile.y = missile.y +10
   if missile.y > 600 then
      missile.y = -230
   end
   if collide(missile,player) then       
   scene(gameover)
   end
   
end

function gameover()
   draw(death)
 
   if key("enter")then
   	scene(game)
    player.x=297
    player.y=402
    missile.x=300
    missile.y=0
   end
end
   
    
