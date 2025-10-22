# VR Pac-Man Technical Specification

## 1. 开发环境设置

### 1.1 Unity版本要求
- Unity 2021.3 LTS 或更高版本
- URP (Universal Render Pipeline) 渲染管线
- Android Build Support模块

### 1.2 PICO SDK集成
- PICO Unity Integration SDK 3.3.0 或更高版本
- XR Interaction Toolkit 2.3.0 或更高版本
- OpenXR Plugin

### 1.3 开发工具
- Unity Hub (用于管理Unity版本)
- PICO Developer Portal账户
- Android SDK & NDK (通过Unity安装)
- PICO Device (用于测试)

## 2. 项目结构

```
pico_pacman/
├── Assets/
│   ├── Scenes/
│   │   ├── MainScene.unity
│   │   └── MenuScene.unity
│   ├── Scripts/
│   │   ├── Player/
│   │   ├── AI/
│   │   ├── GameSystem/
│   │   ├── UI/
│   │   └── Utils/
│   ├── Prefabs/
│   ├── Materials/
│   ├── Models/
│   ├── Textures/
│   ├── Audio/
│   └── Plugins/
├── Packages/
├── ProjectSettings/
└── docs/
```

## 3. 核心系统实现

### 3.1 VR输入系统

#### 传送移动系统
```csharp
// 传送控制器组件
public class TeleportationController : MonoBehaviour
{
    // 射线投射
    // 传送点标记
    // 传送动画
}
```

#### 平滑移动系统 (可选)
```csharp
// 平滑移动控制器组件
public class SmoothMovementController : MonoBehaviour
{
    // 摇杆输入处理
    // 防晕动辅助 (隧道视觉/snap turn)
}
```

### 3.2 迷宫系统

#### 迷宫生成器
```csharp
// 迷宫数据结构
public class MazeGenerator : MonoBehaviour
{
    // 迷宫布局数据
    // 路径生成算法
    // 碰撞体生成
}
```

#### 豆子管理系统
```csharp
// 豆子管理器
public class PelletManager : MonoBehaviour
{
    // 普通豆子生成
    // 能量豆生成
    // 豆子收集检测
}
```

### 3.3 玩家系统

#### 玩家控制器
```csharp
// 玩家角色控制
public class PlayerController : MonoBehaviour
{
    // 移动控制
    // 碰撞检测
    // 生命值管理
    // 分数计算
}
```

### 3.4 鬼魂AI系统

#### 基础鬼魂类
```csharp
// 鬼魂基类
public class Ghost : MonoBehaviour
{
    // 移动逻辑
    // 状态管理 (普通/受惊/回家)
    // AI模式切换
}
```

#### 各种鬼魂实现
```csharp
// Blinky (红色) - 追踪者
public class Blinky : Ghost {}

// Pinky (粉色) - 伏击者
public class Pinky : Ghost {}

// Inky (蓝色) - 随机者
public class Inky : Ghost {}

// Clyde (橙色) - 胆小者
public class Clyde : Ghost {}
```

### 3.5 游戏状态管理

#### 游戏管理器
```csharp
// 游戏主控制器
public class GameManager : MonoBehaviour
{
    // 游戏状态管理
    // 关卡进度控制
    // 分数系统
    // 生命系统
}
```

## 4. PICO SDK集成指南

### 4.1 项目设置步骤

1. 创建新的Unity项目 (3D URP模板)
2. 导入PICO Unity Integration SDK
3. 配置XR Plugin Management
4. 设置Player Settings for Android
5. 配置PICO设备支持

### 4.2 必要的Unity设置

#### Player Settings配置
- Company Name: `YourCompany`
- Product Name: `PicoPacMan`
- Version: `1.0`
- Bundle Identifier: `com.yourcompany.picopacman`
- Color Space: `Linear`
- Target Architectures: `ARM64`
- Graphics APIs: `OpenGLES3`
- Minimum API Level: `Android 7.0 (API level 24)`

#### XR Settings配置
- Virtual Reality Supported: `Enabled`
- Stereo Rendering Mode: `Multi Pass`
- 其他XR相关设置

### 4.3 PICO特定配置

#### PICO Settings配置
- Device Model: `PICO 4` 或 `PICO 3`
- Render Quality Settings
- Tracking Origin Type
- 其他PICO特定选项

## 5. 性能优化指南

### 5.1 渲染优化
- 使用LOD系统优化远处物体
- 合理设置渲染距离
- 使用Occlusion Culling
- 优化材质和纹理

### 5.2 物理优化
- 简化碰撞体形状
- 合理设置物理更新频率
- 使用Layer-based碰撞检测

### 5.3 内存优化
- 对象池管理 (豆子、特效等)
- 及时释放未使用的资源
- 避免频繁的内存分配

### 5.4 VR特定优化
- 保持稳定的90fps帧率
- 减少延迟和抖动
- 优化头部追踪性能

## 6. 测试与调试

### 6.1 设备测试
- 在PICO 3/4设备上测试
- 检查控制器输入响应
- 验证传送和平滑移动功能
- 测试所有游戏机制

### 6.2 性能测试
- 帧率稳定性测试
- 内存使用情况监控
- 电池消耗评估
- 发热情况监测

### 6.3 用户体验测试
- 晕动症测试
- 操作舒适度评估
- UI可读性测试
- 音频空间感测试

## 7. 构建与部署

### 7.1 构建设置
- Build Platform: `Android`
- Texture Compression: `ASTC`
- Build System: `Gradle`
- 其他构建选项

### 7.2 签名配置
- 创建Keystore文件
- 设置签名信息
- 配置应用权限

### 7.3 部署到设备
- USB调试模式启用
- ADB安装命令
- PICO Device Manager使用

## 8. 故障排除

### 8.1 常见问题
- 应用启动失败
- 控制器输入无响应
- 显示异常或黑屏
- 音频问题

### 8.2 调试工具
- Unity Profiler for VR
- PICO Debugger
- Logcat日志查看
- 设备性能监控

## 9. 发布准备

### 9.1 内容审核
- 符合PICO内容政策
- 版权和商标检查
- 年龄评级确认

### 9.2 应用商店提交
- PICO应用商店注册
- 应用信息填写
- 截图和宣传材料
- 版本发布流程