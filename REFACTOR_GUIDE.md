# 当前项目的代码分层

本次按 `CodePathTraveler/FRAMEWORK_GUIDE.md` 的分层和通信原则适配现有 2D 游戏。
键盘绑定和旧 Input Manager 保留，没有引入新输入系统、职业、战斗或鼠标行动菜单。
代码兼容当前 Unity 2022.3 / C# 9，不使用参考项目中的 `global using` 和文件范围命名空间。

## 目录归属

```text
Assets/Scripts
├─ MFramework
│  ├─ Event             IEvent、IEventReceiver、EventBus
│  └─ Singleton         Singleton<T>
├─ Utility              GameMode 枚举
├─ Game
│  ├─ Input             InputSystemController
│  ├─ GameMode          GameModeManager、模式请求和变化事件
│  ├─ Character         PlayerDefinitionSo
│  ├─ Physics           IMassProvider、MassUtility
│  ├─ Level             LevelFlow、Next
│  ├─ Interaction
│  │  ├─ Bottle         Bottle、BottleTigger、Cap、BottleStateChangedEvent
│  │  │  └─ Presentation BottlePresenter
│  │  └─ Ice            Ice
│  ├─ Platform          Floor、MoveOlatformer、PlatformMassChangedEvent
│  │  └─ Presentation   Rock_1Animator
│  ├─ Audio             AudioManager、SoundRequestedEvent
│  ├─ Camera            ParallaxEffect、MouseParallaxEffect
│  └─ Debug             Test1
├─ Player               PlayerMoveControl、PlayerState、PlayerLevelControl、玩家状态及关卡事件
│  └─ Presentation      PlayerAnimations
└─ UI
   ├─ Menu              MenuControl、MenuController、VolumeController
   ├─ HUD               LevelManager、MassDisplay、UIDisplay
   └─ Effects           GlowAnimator

Assets/Data/Player      玩家运动配置资产
```

`MovingPlatform.cs` 搬迁后命名为 `MoveOlatformer.cs`，与原有类名一致；类名和脚本 GUID 没有改变。
所有原有脚本及其 `.meta` 成对迁移，第三方资源代码保持原目录。
当前自有代码共 41 个 C# 文件。原目录中已替换的历史注释实现和空生命周期方法已清理。

## 运行职责

- `InputSystemController` 是设备读取入口，通过静态语义属性提供移动、跳跃、释放、重开和切关输入；挂在 Menu Prefab 上的组件发布 Escape 暂停请求。暂停期间这些玩法输入被屏蔽，Escape 和 UI 仍可使用。
- `GameModeManager` 是场景内的模式所有者，维护 `Playing / Pause` 和 `Time.timeScale`，发布模式变化。
- `MenuControl` 提交模式或关卡请求，根据模式事件显示暂停面板；原有按钮方法名保留。
- `PlayerMoveControl` 消费输入和玩家状态，执行 Rigidbody2D 移动及地面检测。
- `PlayerState` 持有三态和飞行标志，处理释放、补水、冰块生成及环境触发；不再依赖移动组件。
- `PlayerLevelControl` 处理死亡、终点、重开和切关输入；死亡/过关只发起一次延迟切换。
- `LevelFlow` 集中处理场景加载；`LevelManager` 仍只显示关卡名称。
- `PlayerStateChangedEvent` 携带玩家、旧状态和新状态，瓶子按玩家身份筛选事件。
- 玩家音效通过 `SoundRequestedEvent` 交给 `AudioManager`，暂停音乐通过模式事件响应。
- `IMassProvider` 统一提供玩家、瓶子和冰块质量，`MassUtility` 汇总子层级；`Floor` 负责平台移动和质量变化事件。
- `MassDisplay`、`Rock_1Animator` 根据平台事件更新文字和动画，重新启用时读取当前状态。
- `Bottle` 负责灌注和物理行为，`BottlePresenter` 根据瓶子事件更新颜色。
- `PlayerAnimations` 根据玩家状态和死亡/终点事件更新动画，不再逐帧轮询业务组件。
- 音量 UI 通过 `AudioManager` 接口调整音量，滑块监听随组件启用/禁用注册和注销。

基础单例不自动创建对象、不默认跨场景保留；只有 `AudioManager` 延续原来的 `DontDestroyOnLoad` 行为。
模式组件和输入组件已挂到 Menu Prefab，关卡控制组件已挂到 Player Prefab，无需手动补挂。
瓶子表现组件已通过 Unity MCP 挂到 Bottle Prefab。27 个自有关卡场景完成组件和必要引用扫描；11 个场景中原先为空的 25 处视差背景玩家引用已绑定到各场景唯一玩家。

## 玩家配置迁移

`PlayerMoveControl.definition` 指向 `PlayerDefinitionSo`，配置资产只保存运动参数，不保存运行时状态。

| 配置 | 满态速度 | 空态速度 | 飞行速度 | 满态跳跃 | 空态跳跃 | 飞行力度 |
|---|---:|---:|---:|---:|---:|---:|
| PlayerDefault.asset | 4 | 6 | 6 | 11.5 | 18.5 | 5 |
| PlayerTest1_9.asset | 4 | 6 | 6 | 11.5 | 19 | 5 |
| PlayerTest2_1.asset | 4 | 6 | 6 | 11.5 | 18.5 | 6.5 |

Test2.4 原有的飞行速度覆盖值与默认值相同，现引用默认配置。玩家初始状态和其余场景参数保持原值。
修改共享配置会影响所有引用它的关卡；关卡专用参数应使用独立配置资产。

## 验证记录与人工验收

- 使用当前 Unity 自带编译器和项目程序集引用编译全部 Assets C# 脚本，通过。
- 对照迁移前备份检查 22 个原脚本 GUID、111 个场景/Prefab 的既有脚本引用、对象 ID 和按钮绑定，通过。
- 检查默认运动参数及三个场景的参数覆盖迁移，通过。
- 事件总线的 7 项临时运行检查通过：无接收者发布、重复订阅、自身退订不跳过下一个接收者、新订阅延至下轮通知、下一轮通知、回调中取消其他接收者，以及最后一个接收者退订后再次发布。
- 本轮开始前已有的四个 Packages/ProjectSettings 文件修改未被覆盖；以本轮开始时的 ZIP 为对照逐字节验证，包含用户已添加的 MCP 包配置。
- 已通过本地 stdio MCP 连接 Unity 的 `Project@dd973257`，完成资源刷新、编译和原生 Prefab/场景引用迁移。原生扫描未发现缺失脚本或所检查的必要引用缺失。
- Play Mode 中 22 项核心断言通过：玩家三态质量、瓶子灌注及摩擦材质、瓶子颜色事件、嵌套质量、平台去重与文字/动画更新、飞行释放、冰块生成、暂停/恢复、死亡及终点事件。
- 22 个构建场景依次加载完成，玩家配置、状态/关卡组件及玩法场景的模式/输入组件检查通过。每场景等待约 0.75 秒，属于启动检查，不代表完整通关验收。
- 验证结束已退出 Play Mode，回到 `Test1.1`；场景无未保存修改。
- `git diff --check` 报告 Unity 序列化空字段的行尾空格，以及本轮前已有的 ProjectSettings 行尾空格；未对这些资源和用户配置做额外格式化。

### 已知附带问题

Console 仍报告 `Trap` / `Trap (1)` 的 `AnimationEvent has no function name specified!`。第三方素材 `Assets/Gothicvania Collection 2/Artwork/ENVIRONMENTS/Volcanic Area/lava-tile/lava Animation.anim` 包含一个空函数名事件；该动画文件与 Git 基线一致，本次没有修改第三方动画。不能将本次验证表述为 Console 零错误。

### 备份与验证证据

本轮整体迁移前备份：`Logs/RefactorBackups/before-full-migration-20260913-201151.zip`，包含迁移前自有代码、配置、场景/Prefab 及当时的 Packages/ProjectSettings。它对应本轮开始状态，早于本轮的重构改动不在其回退范围内。回退时应先比较文件差异，避免覆盖后续编辑。

本轮原始验证结果保存在 `Logs/RefactorValidation/20260913/`。临时检查程序位于 `Temp/`，不会打包进游戏，也没有新增测试依赖。

在 Unity 刷新资源后，从 MainScene 开始，依次检查：菜单进入 Test1.1、移动/跳跃、E 释放与瓶子灌注、飞行、冷区生成冰块、平台质量、Escape 暂停/恢复、R 重开、陷阱死亡和终点切关。单独检查 Test1.9、Test2.1 的专用配置。
