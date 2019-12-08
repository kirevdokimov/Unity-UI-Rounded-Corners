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

inline float2 translate(float2 samplePosition, float2 offset){
    return samplePosition - offset;
}

float intersect(float shape1, float shape2){
    return max(shape1, shape2);
}

float2 rotate(float2 samplePosition, float rotation){
    const float PI = 3.14159;
    float angle = rotation * PI * 2 * -1;
    float sine, cosine;
    sincos(angle, sine, cosine);
    return float2(cosine * samplePosition.x + sine * samplePosition.y, cosine * samplePosition.y - sine * samplePosition.x);
}

float circle(float2 position, float radius){
    return length(position) - radius;
}

float CalcAlphaForIndependentCorners(float2 samplePosition, float2 halfSize, float4 rect2props, float4 r){

    samplePosition = (samplePosition - .5) * halfSize * 2;

    float r1 = rectangle(samplePosition, halfSize);
                
    float2 r2Position = rotate(translate(samplePosition, rect2props.xy), .125);
    float r2 = rectangle(r2Position, rect2props.zw);
    
    float2 circle0Position = translate(samplePosition, float2(-halfSize.x + r.x, halfSize.y - r.x));
    float c0 = circle(circle0Position, r.x);
    
    float2 circle1Position = translate(samplePosition, float2(halfSize.x - r.y, halfSize.y - r.y));
    float c1 = circle(circle1Position, r.y);
    
    float2 circle2Position = translate(samplePosition, float2(halfSize.x - r.z, -halfSize.y + r.z));
    float c2 = circle(circle2Position, r.z);
    
    float2 circle3Position = translate(samplePosition, -halfSize + r.w);
    float c3 = circle(circle3Position, r.w);
        
    float dist = max(r1,min(min(min(min(r2, c0), c1), c2), c3));
    return AntialiasedCutoff(dist);
}