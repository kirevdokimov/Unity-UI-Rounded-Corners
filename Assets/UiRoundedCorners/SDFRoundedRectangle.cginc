// SDF Function for rectangle
float rectangle(float2 samplePosition, float2 halfSize){
    // X component represents signed distance to closest vertical edge of rectangle
    // Same for Y but for closest horizontal edge
    // HalfSize represents two distances from each axis of 2d space to a сorresponding edge
    float2 distanceToEdge = abs(samplePosition) - halfSize;
    // max(n, 0) to remove distances that signed with minus (distances inside rectangle)
    // length to calculate distance from outside (distances that > 0) to rectangle
    float outsideDistance = length(max(distanceToEdge, 0));
    // max(x,y) is a cheap way to calculate distance to closest edge inside rectangle
    // with min we just make sure that inside distances would not impact on outside distances
    float insideDistance = min(max(distanceToEdge.x, distanceToEdge.y), 0);
    return outsideDistance + insideDistance;
}

// An extension of rectangle() function to modify signed distance (effect as a wrap around rectangle)
float roundedRectangle(float2 samplePosition, float absoluteRound, float2 halfSize){ 
    // subtrancting value from final distance effects like a wrap around rectangle, so
    // the solution is to decrease actual rectangle with `absoluteRound` value 
    // and then make an effect of wrap with size of `absoluteRound`
    return rectangle(samplePosition, halfSize - absoluteRound) - absoluteRound;
}

// Smoothstep function with antialiasing
float AntialiasedCutoff(float distance){
    float distanceChange = fwidth(distance) * 0.5;
    return smoothstep(distanceChange, -distanceChange, distance);
}

float CalcAlpha(float2 samplePosition, float2 size, float radius){
    // -.5 = translate origin of samplePositions from (0, 0) to (.5, .5)
    // because for Image component (0,0) is bottom-right, not a center
    // * size = scale samplePositions to localSpace of Image with this size
    float2 samplePositionTranslated = (samplePosition - .5) * size;
    float distToRect = roundedRectangle(samplePositionTranslated, radius * .5, size * .5);
    return AntialiasedCutoff(distToRect);
}