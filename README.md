# Unity-UI-Rounded-Corners
These components and shaders allows you to add rounded corners to UI elements!
---

![](gif-00.gif)

## How to install
### Package Manager
- Open `%projectname%/Packages/manifest.json`
- Add following :
```
{
  "scopedRegistries": [
    {
      "name": "XCrew",
      "url": "http://xcrew.dev",
      "scopes": [
        "com.nobi"
      ]
    }
  ],
  "dependencies": {
    "com.nobi.roundedcorners": "1.0.1",
    ...
  }
}
```

### Unity Package
Get `.unitypackage` from [releases](https://github.com/Nobinator/Unity-UI-Rounded-Corners/releases)

## How to use
- Create Panel
- Replace `Image` with `Image With Rounded Corners`
- Сreate new material with one of the following shaders
  - `UI/RoundedCorners/Color`
  - `UI/RoundedCorners/Texture`
- Attach material to `Image With Rounded Corners`
- Profit

# Features
## Keeps round while resizing
![](gif-01.gif)
## Better quality than sprites
![](image-00.png)
## Supports Unity Mask
![](gif-02.gif)
## Supports Tint
![](gif-04.gif)
