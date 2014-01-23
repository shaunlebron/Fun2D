title = love.graphics.setCaption

key = function (k)
	if k == "enter" then
		return love.keyboard.isDown("return") or love.keyboard.isDown("kpenter")
	end
   	return love.keyboard.isDown(k)
end

random = math.random

list = function(n)
	a = {}
	for i=1,n do
		a[i] = 0
	end
	return a
end