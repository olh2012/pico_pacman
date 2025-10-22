# VR Pac-Man Project Structure

## 项目文件夹结构

```
pico_pacman/
├── Assets/
│   ├── Scenes/
│   │   ├── MainScene.unity
│   │   └── MenuScene.unity
│   ├── Scripts/
│   │   ├── Player/
│   │   │   ├── PlayerController.cs
│   │   │   ├── TeleportationController.cs
│   │   │   └── SmoothMovementController.cs
│   │   ├── AI/
│   │   │   ├── Ghost.cs
│   │   │   ├── Blinky.cs
│   │   │   ├── Pinky.cs
│   │   │   ├── Inky.cs
│   │   │   ├── Clyde.cs
│   │   │   └── GhostAIManager.cs
│   │   ├── GameSystem/
│   │   │   ├── GameManager.cs
│   │   │   ├── MazeGenerator.cs
│   │   │   ├── PelletManager.cs
│   │   │   ├── ScoreManager.cs
│   │   │   └── GameState.cs
│   │   ├── UI/
│   │   │   ├── UIManager.cs
│   │   │   ├── VRMenu.cs
│   │   │   └── HUDController.cs
│   │   └── Utils/
│   │       ├── Singleton.cs
│   │       └── Extensions.cs
│   ├── Prefabs/
│   │   ├── Player/
│   │   ├── Ghosts/
│   │   ├── Pellets/
│   │   ├── UI/
│   │   └── Environment/
│   ├── Materials/
│   ├── Models/
│   ├── Textures/
│   ├── Audio/
│   └── Plugins/
├── Packages/
├── ProjectSettings/
├── docs/
│   ├── design_document.md
│   ├── technical_spec.md
│   ├── setup_guide.md
│   └── project_structure.md
└── README.md
```

## 脚本说明

### Player文件夹
- `PlayerController.cs`: 玩家角色的主要控制逻辑
- `TeleportationController.cs`: 传送移动系统实现
- `SmoothMovementController.cs`: 平滑移动系统实现

### AI文件夹
- `Ghost.cs`: 鬼魂基类，包含通用行为
- `Blinky.cs`: 红色鬼魂，追踪者行为
- `Pinky.cs`: 粉色鬼魂，伏击者行为
- `Inky.cs`: 蓝色鬼魂，随机者行为
- `Clyde.cs`: 橙色鬼魂，胆小者行为
- `GhostAIManager.cs`: 鬼魂AI管理器

### GameSystem文件夹
- `GameManager.cs`: 游戏主控制器，管理游戏状态
- `MazeGenerator.cs`: 迷宫生成和管理
- `PelletManager.cs`: 豆子系统管理
- `ScoreManager.cs`: 分数系统管理
- `GameState.cs`: 游戏状态枚举和管理

### UI文件夹
- `UIManager.cs`: UI系统管理器
- `VRMenu.cs`: VR菜单系统
- `HUDController.cs`: 抬头显示控制器

### Utils文件夹
- `Singleton.cs`: 单例模式基类
- `Extensions.cs`: 扩展方法集合

## Prefabs说明

### Player子文件夹
- 包含玩家角色预制体

### Ghosts子文件夹
- 包含四种鬼魂预制体

### Pellets子文件夹
- 包含普通豆子和能量豆预制体

### UI子文件夹
- 包含UI元素预制体

### Environment子文件夹
- 包含环境元素预制体（迷宫墙壁等）

## 开发规范

### 命名规范
- 脚本文件名与类名保持一致
- 使用PascalCase命名法
- 变量名使用camelCase命名法
- 常量使用UPPER_CASE命名法

### 代码组织
- 每个类单独一个文件
- 相关功能组织在对应文件夹中
- 使用命名空间组织代码

### 版本控制
- 使用Git进行版本控制
- 遵循Git Flow工作流
- 定期提交并编写清晰的提交信息