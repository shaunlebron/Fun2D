_image_data = {}
_images = {}

function _makeTransparent(x,y,r,g,b,a)
   if r==192 and g==192 and b==192 then
      a=0
   end
   return r,g,b,a
end

function _openimage(name)
    local source = love.image.newImageData(name..".png")
    local w, h = source:getWidth(), source:getHeight()
    
    -- Find closest power-of-two.
    local wp = math.pow(2, math.ceil(math.log(w)/math.log(2)))
    local hp = math.pow(2, math.ceil(math.log(h)/math.log(2)))
    
    _image_data[name] = love.image.newImageData(wp, hp)
    _image_data[name]:paste(source, 0, 0)
    _image_data[name]:mapPixel(_makeTransparent)
    
    _images[name] = love.graphics.newImage(_image_data[name])
end

function _addImages()
	local lfs = love.filesystem
	local files = lfs.enumerate("")
	
	for i,v in ipairs(files) do
		if v ~= "blank" then
			local name = string.lower(v)
			if string.find(name,".png") then
				_openimage(string.sub(name,1,-5))
			end
		end
	end
end
_addImages()

function _intersect(left1,top1,right1,bottom1,left2,top2,right2,bottom2)
	if (right1 < left2 or right2 < left1 or bottom1 < top2 or bottom2 < top1) then
		return nil, nil, nil, nil
	end
	
	return (left1 < left2) and left2 or left1,
		(top1 < top2) and top2 or top1,
		(right1 < right2) and right1 or right2,
		(bottom1 < bottom2) and bottom1 or bottom2
end

function _imageCollide(image1,image2,left1,top1,left2,top2)
	local right1 = left1 + _images[image1]:getWidth()
	local bottom1 = top1 + _images[image1]:getHeight()
	local right2 = left2 + _images[image2]:getWidth()
	local bottom2 = top2 + _images[image2]:getHeight()
	
	local left,top,right,bottom = 
		_intersect(left1,top1,right1,bottom1,left2,top2,right2,bottom2)
	
	if left == nil then
		return false
	end
	
	local step = 1
	local z,a1,a2,r,g,b
	for x=left+1,right-1,step do
		for y=top+1,bottom-1,step do
			r,g,b,a1 = _image_data[image1]:getPixel(x-left1,y-top1)
			z,z,z,a2 = _image_data[image2]:getPixel(x-left2,y-top2)
			if a1 == nil or a2 == nil then
				
			elseif a1 ~= 0 and a2 ~= 0 then
				return true
			end
		end
	end
	
	return false
end

image = function(name)
	return { x = 0, y = 0, image = name	}
end

collide = function(o1,o2)
	return _imageCollide(o1.image,o2.image,o1.x,o1.y,o2.x,o2.y)
end

draw = function(i, c)
	if c then
		color(c)
	else
		color("white")
	end
	love.graphics.draw(_images[i.image],i.x,i.y)
end