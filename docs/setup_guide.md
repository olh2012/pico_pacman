# PICO VR Pac-Man 开发环境设置指南

## 1. 前置要求

### 1.1 硬件要求
- 支持Mac/Windows/Linux的开发电脑
- PICO 3 或 PICO 4 VR头显 (用于测试)
- USB-C 数据线 (用于连接设备)
- 稳定的网络连接

### 1.2 软件要求
- Unity Hub
- Unity 2021.3 LTS 或更新版本
- Android SDK & NDK
- PICO Developer Portal账户

## 2. Unity安装与配置

### 2.1 安装Unity Hub
1. 访问 [Unity官网](https://unity.com/download)
2. 下载Unity Hub
3. 安装Unity Hub

### 2.2 通过Unity Hub安装Unity
1. 打开Unity Hub
2. 点击"Installs"标签
3. 点击"Install Editor"
4. 选择Unity 2021.3 LTS版本
5. 确保选中以下模块:
   - Android Build Support
   - iOS Build Support (可选)
   - WebGL Build Support (可选)
6. 点击"Install"开始安装

## 3. PICO SDK安装

### 3.1 注册PICO开发者账户
1. 访问 [PICO Developer Portal](https://developer-global.pico-interactive.com/)
2. 注册开发者账户
3. 登录并访问SDK下载页面

### 3.2 下载PICO Unity Integration SDK
1. 在开发者门户找到"PICO Unity Integration SDK"
2. 下载最新版本 (推荐3.3.0或更高)
3. 解压下载的文件

### 3.3 导入SDK到Unity项目
1. 打开Unity项目
2. 在菜单栏选择"Assets > Import Package > Custom Package"
3. 选择解压后的PICO SDK包
4. 点击"Import"导入所有资源

## 4. XR Interaction Toolkit安装

### 4.1 通过Package Manager安装
1. 在Unity中打开"Window > Package Manager"
2. 点击左上角"+"按钮
3. 选择"Add package from git URL"
4. 输入: `com.unity.xr.interaction.toolkit`
5. 点击"Add"安装

### 4.2 或者通过Unity Registry安装
1. 在Package Manager中选择"Packages: Unity Registry"
2. 搜索"XR Interaction Toolkit"
3. 点击"Install"安装

## 5. 项目配置

### 5.1 XR Plugin Management设置
1. 在Package Manager中安装"XR Plugin Management"
2. 在Project Settings中找到"XR Plug-in Management"
3. 启用"Initialize XR on Startup"
4. 在Android设置中启用PICO支持

### 5.2 Player Settings配置
在"Edit > Project Settings > Player"中配置:

#### Other Settings:
- Company Name: `YourCompany`
- Product Name: `PicoPacMan`
- Version: `1.0`
- Bundle Identifier: `com.yourcompany.picopacman`
- Color Space: `Linear`
- Target Architectures: `ARM64`

#### Resolution and Presentation:
- Default Orientation: `Landscape Left`
- Render Outside Safe Area: `Enabled`

#### XR Settings:
- Virtual Reality Supported: `Enabled`
- Stereo Rendering Mode: `Multi Pass`

### 5.3 PICO特定设置
1. 在Project Settings中找到"PICO Settings"
2. 设置Device Model为"PICO 4"或"PICO 3"
3. 配置其他PICO特定选项

## 6. PICO设备设置

### 6.1 启用开发者模式
1. 在PICO头显中打开"Settings"
2. 进入"About"页面
3. 连续点击"Software Version"直到出现开发者选项
4. 启用"Developer Mode"

### 6.2 USB调试设置
1. 在Settings中找到"Developer Options"
2. 启用"USB Debugging"
3. 通过USB-C线连接设备到电脑

### 6.3 验证设备连接
1. 在终端中运行: `adb devices`
2. 确认设备出现在设备列表中

## 7. 测试运行

### 7.1 构建APK
1. 在Unity中选择"File > Build Settings"
2. 选择"Android"平台
3. 点击"Switch Platform"
4. 连接PICO设备
5. 点击"Build And Run"

### 7.2 在设备上测试
1. 确认APK成功安装到设备
2. 启动应用并测试基本功能
3. 检查控制器输入是否正常工作

## 8. 常见问题解决

### 8.1 Unity版本兼容性
- 确保使用Unity 2021.3 LTS或更新版本
- 避免使用beta或experimental版本

### 8.2 SDK导入问题
- 确保完全导入所有SDK组件
- 检查是否有重复的脚本或资源

### 8.3 构建错误
- 检查Player Settings配置
- 确认Android SDK路径正确
- 验证签名配置

### 8.4 设备连接问题
- 确认USB调试已启用
- 检查USB线缆连接
- 重启设备和开发电脑

## 9. 下一步

完成环境设置后，您可以:
1. 克隆项目仓库
2. 在Unity中打开项目
3. 导入所需的额外资源
4. 开始开发VR Pac-Man游戏功能

参考文档:
- [PICO Developer Documentation](https://developer-global.pico-interactive.com/)
- [Unity XR Documentation](https://docs.unity3d.com/Manual/XR.html)
- [XR Interaction Toolkit Documentation](https://docs.unity3d.com/Packages/com.unity.xr.interaction.toolkit@2.3/manual/index.html)