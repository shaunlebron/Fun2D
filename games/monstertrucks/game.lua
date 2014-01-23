function start()
  monstertruck = image("road")
  freddy = image("freddy")
  rhino = image("rhino")
  dead  = image("gameover")
  
  freddy.x = 212
  freddy.y = 420
  rhino.x = 384
  rhino.y = 25
  scene(game)
end

function game()
  draw(monstertruck)
  draw(freddy)
  draw(rhino)
  
  
  if key("left")then
    freddy.x = freddy.x -10
  end

  if key("right")then
    freddy.x = freddy.x +10
  end
  
  
  if key("up")then
    freddy.y = freddy.y -10
   end
   
  if key("down")then
    freddy.y = freddy.y +10
   end
   
  if key("f")then
    rhino.x = rhino.x -10
   end
   
  if key("r")then
    rhino.x = rhino.x +10  
  end
  
  if key("e")then
    rhino.y = rhino.y -10
  end
  
  if key("d")then 
    rhino.y = rhino.y +10
  end
  
  if collide(rhino,freddy)then
     scene(gameover)      
  end
  
  
end

function gameover()
    draw(dead)
    
  if key("enter")then
    scene(game)
    freddy.x = 200
    freddy.y = 300
    rhino.x = 300
    rhino.y = 0
   end 
    
 end