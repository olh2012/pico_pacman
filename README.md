# pico_pacman
PICO VR版吃豆人游戏

## 项目概述
将经典的《吃豆人》游戏体验从2D平面无缝过渡至3D沉浸式虚拟现实环境，在保留原版游戏核心乐趣的同时，充分利用VR技术的交互特性，为玩家带来全新的、更具临场感的追逐与探索体验。

## 技术栈
- Unity 3D 引擎 (URP渲染管线)
- PICO Unity Integration SDK
- C# 编程语言
- XR Interaction Toolkit

## 核心功能
1. **VR移动控制**
   - 传送移动（推荐方案）
   - 摇杆平滑移动（可选方案，带防晕动辅助）

2. **游戏机制**
   - 经典吃豆玩法
   - 四种鬼魂AI行为模式（Blinky, Pinky, Inky, Clyde）
   - 能量豆变身机制
   - 分数系统与生命值管理

3. **视觉与音效**
   - 3D立体迷宫设计
   - 第三人称追随视角
   - 3D空间音频效果
   - 经典背景音乐改编

## 项目结构
```
pico_pacman/
├── Assets/
│   ├── Scenes/
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
├── docs/
│   ├── design_document.md
│   ├── technical_spec.md
│   ├── setup_guide.md
│   ├── project_structure.md
│   └── development_roadmap.md
└── README.md
```

## 开发环境设置

详细设置指南请参考 [setup_guide.md](docs/setup_guide.md)

### 快速开始
1. 安装Unity Hub和Unity 2021.3 LTS
2. 克隆此仓库到本地
3. 使用Unity Hub打开项目
4. 从PICO开发者门户下载并导入PICO Unity Integration SDK
5. 安装XR Interaction Toolkit包
6. 配置Player Settings和XR Plugin Management

## 已实现功能

### 核心系统
- [x] GameManager (游戏状态管理)
- [x] PlayerController (玩家控制)
- [x] Ghost base class (鬼魂基类)
- [x] Ghost implementations (四种鬼魂)
- [x] GhostManager (鬼魂协调管理)
- [x] MazeGenerator (迷宫生成器)
- [x] PelletManager (豆子管理)
- [x] TeleportationController (传送控制)
- [x] SmoothMovementController (平滑移动)
- [x] UIManager (用户界面管理)
- [x] ScoreManager (分数管理)
- [x] VRMenu (VR菜单系统)
- [x] HUDController (抬头显示)
- [x] Singleton pattern (单例模式)
- [x] Utility extensions (工具扩展)

### 文档
- [x] 设计文档
- [x] 技术规范
- [x] 设置指南
- [x] 项目结构说明
- [x] 开发路线图
- [x] 测试计划
- [x] 用户指南

## 开发进度
- [x] 项目初始化和文档
- [x] 核心系统架构
- [x] 完整鬼魂AI系统
- [ ] 迷宫设计与实现
- [ ] 游戏机制实现
- [ ] VR控制集成
- [ ] UI/UX设计
- [ ] 音效集成
- [ ] 性能优化
- [ ] 测试与调试

## 构建和部署

### 构建到PICO设备
1. 连接PICO设备到电脑并启用开发者模式
2. 在Unity中选择File > Build Settings
3. 选择Android平台并切换
4. 配置Player Settings (参考技术规范文档)
5. 点击Build And Run

## 贡献指南

1. Fork本仓库
2. 创建功能分支 (`git checkout -b feature/AmazingFeature`)
3. 提交更改 (`git commit -m 'Add some AmazingFeature'`)
4. 推送到分支 (`git push origin feature/AmazingFeature`)
5. 开启Pull Request

## 许可证
本项目采用MIT许可证 - 查看 [LICENSE](LICENSE) 文件了解详情

## 联系方式
项目链接: [https://github.com/yourusername/pico_pacman](https://github.com/yourusername/pico_pacman)