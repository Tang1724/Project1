# 第一章：熔岩遗迹场景表现

## 本次范围

以 `Assets/Chapter one/Test1.10.unity` 为视觉示范，已同步到 `Test1.1`、`Test1.3`～`Test1.9`，使用 `Assets/Art/Gothicvania Collection 2` 素材。用户明确排除了 `Test1.222222`；项目没有 `Test1.2.unity`，本次没有新增、改名或调整构建列表。数字质量牌接入共享 `Move`、`Move1`、`Move2` 三类电梯预制体。

## 角色

- 使用 Gothicvania 的 Monk，无武器；接入待机、行走、跳跃、下落、受伤和终点表现。移动状态复用直立 Walk 动画，避免 Run 前倾姿势造成突然变矮；待机播放速率为 6 帧/秒，移动为 8 帧/秒，没有添加攻击输入。
- `Traveler Visual` 是玩家的表现子物体，原来的玩家移动、状态、关卡控制及物理组件保留。
- 满水、空水、飞行态使用不同色调及头顶光点；倒水时播放粒子，走路和落地带少量尘埃。
- 动画控制器：`Assets/Animations/VolcanicTraveler/Traveler.controller`。起跳和下落使用项目内的 `Airborne.anim`，复用 Monk 跳跃贴图并统一为 32 PPU，避免原 100 PPU 贴图切换时缩小；腾空播放速率为 8 帧/秒。
- 玩家及其表现子物体使用现有 `HIGH` 排序层，背景使用 `BackGround`，道路及电梯使用 `MID`，使人物保持可见。
- `PlayerAnimations` 的表现引用配置于示范场景及本次同步的 8 个场景，其他未配置场景继续使用原有 Animator。

## 场景分组

`Volcanic Ruins - Presentation` 下的内容均为装饰，没有 Collider2D 或 Rigidbody2D。

| 分组 | 内容 |
|---|---|
| Distant Ruins | 古老石像、枯树、破损守卫石雕 |
| Platform Details | 蕨草、石块、木桶、陶罐、旗帜和 9 处动画火把 |
| Atmosphere | 火把余烬、暖色光晕、飘灰及火山薄雾 |

场景图像来自素材包内的 Cemetery、Mystic Forest、Magic Castle、Volcanic Area 和 VFX。柔光及角色粒子另使用 Unity 内置粒子纹理；没有下载或生成外部美术图片。

背景降低亮度，让地面边缘、水源、瓶子和出口保持可辨认。旗帜、薄雾和火光通过 `AmbientMotion` 产生轻微变化，暂停时随游戏时间停止。

## 验证与边界

- Unity 编译通过；本轮 28 项运行检查通过，包含待机、移动、起跳、下落、落地的动画切换与缩放，人物排序，以及电梯上端、中途、下端的台面和链条对齐。
- 场景及资源检查：缺失脚本为 0，腾空帧纹理有效且均为 32 PPU。验收期间 Unity 再次进入运行模式，本轮未强制中断后续试玩来重开场景。
- 原主地形 Tilemap 的碰撞轮廓、电梯碰撞尺寸与载重参数、玩家根物体变换保持原样。三类电梯预制体新增质量牌，其他场景文件未直接修改。
- 素材整理后空气墙原来的资源 GUID 已不在 Assets 中，Unity 保存场景时重新序列化了其 Tile 和碰撞缓存。本次未主动编辑空气墙；仍需人工通关验收边界。
- 尚未完成整关人工通关；自动检查不等同于跳跃手感和解谜难度验收。
- 停止运行时另复现既有 `Floor.OnCollisionExit2D` 第 55 行的 SetParent 报错（停用 Floor 时解除 Bottle 父子关系）；该玩法脚本未在本轮外观修改中改动。

## 道路与电梯本体

- `Grid/Ruins Terrain/Roads and Masonry` 使用 Gothicvania 的 Tomb 与 Magic Castle 拼接 604 个半单位格，区分路面压边、石桥端头、砖体填充和立柱侧边；对应 32 种 Tile 资源位于 `Assets/Tilemap/Ruins`。
- 原 `Grid/Tilemap` 保留作为碰撞来源，关闭旧渲染器。新 Tilemap 负责道路外观，不含额外碰撞组件。后续改路线时需同步调整碰撞图与表现图。
- `Move3/Floor/Lift Deck` 替换原换色石块的显示，台面边界对齐现有电梯碰撞体；`Move3/Lift Suspension` 包含固定横梁与两条悬吊链。
- `LiftPresentation` 根据电梯当前高度更新链条长度。原 Floor 的载重判断、移动端点和速度保留。
- 本轮备份：`Logs/RefactorBackups/before-terrain-lift-fix-20260914.zip`。仅包含本轮已有场景、控制器和说明文件。

预览：`Logs/VisualRefresh/terrain-lift-final.png`（运行模式中将电梯置于中途，核对显示）；腾空预览：`Logs/VisualRefresh/terrain-airborne-final.png`。验证记录：`Logs/VisualRefresh/validation/`。

## 水源、陶罐与实时质量牌

- 水源改用 Magic Castle 的动态水纹与遗迹石材组成开放的高位蓄水盆，沿用原 Water 触发范围；石柱及盆体不增加碰撞。
- 当前关卡的两个 Bottle 显示为同素材包的陶罐，保留原物理与装水逻辑。空罐为灰褐色，罐身水位窗为空；满水后罐身变为青蓝色、水位窗填满亮蓝色；飞行态罐身及水位窗为金色。原宝石动画在这两个场景实例上停用。
- 电梯正面的数字牌显示 `当前质量/启动所需质量`；分母读取 `Floor.RequiredMass`。这是启动门槛，并非超载上限。未达到门槛为浅金色，达到门槛为青绿色；数字始终保留，颜色只作辅助。
- `MassDisplay` 继续监听 `PlatformMassChangedEvent`，支持原 Text 和新增 TMP 世界文字；每块牌只响应自己的 Floor。数字牌保持世界尺寸，避免电梯非等比缩放压扁文字。
- `Assets/Prefabs/Lift Mass Plaque.prefab` 已挂到 `Move.prefab`、`Move1.prefab`、`Move2.prefab`，分别支持 1、5、10 的门槛。新增电梯可将该牌作为 Floor 子物体，保持 prefab 的 floor 引用为空即可在运行时自动绑定父级 Floor。当前位置放在平台正面中心。
- 当前关卡原单个屏幕质量文本关闭，改由电梯自身显示；其他旧 HUD 的 Text 引用仍兼容。
- 本轮 29 项运行检查通过，覆盖真实 RecalculateMass 到 UI 的刷新、倒水后减重、分母改变、三种预制体绑定、多个电梯互不串值、陶罐三态与水源补水、水纹动画。测试运行阶段控制台无错误或警告；前述 Floor 停用报错仍是独立已知问题。
- 预览：`Logs/VisualRefresh/water-ui-final.png`。备份：`Logs/RefactorBackups/before-water-ui-20260914.zip`，同时保存了开始时有未保存编辑的场景副本 `Temp/before-water-ui-live.unity`。
- 陶罐辨识度调整：将原罐口小水光改为罐身水位窗，并增强整罐颜色变化。重新打开场景后引用有效，20 项运行检查通过，涵盖两只罐子的三态切换、恢复为空、尺寸及质量不变。三态对比：`Logs/VisualRefresh/vessel-state-comparison.png`；本次备份：`Logs/RefactorBackups/before-vessel-state-visibility-20260914.zip`。
修改前备份：`Logs/RefactorBackups/before-visual-refresh-20260914.zip`。该备份早于素材目录整理，回退时应只比较并恢复本次相关文件，避免覆盖后续素材整理结果。

## 第一章同步、陷阱辨识与完整状态比例（2026-09-14）

| 场景 | 道路表现格 | 水源 | 陶罐 | 质量电梯 | 危险区域表现 |
| --- | ---: | ---: | ---: | ---: | --- |
| Test1.1 | 620 | 1 | 0 | 0 | 2 处熔岩 |
| Test1.3 | 720 | 1 | 0 | 0 | 8 段熔岩、12 格顶刺 |
| Test1.4 | 924 | 1 | 0 | 2 | 5 段熔岩 |
| Test1.5 | 764 | 2 | 0 | 3 | 12 段熔岩 |
| Test1.6 | 744 | 1 | 0 | 1 | 2 段熔岩、1 处失水热区 |
| Test1.7 | 632 | 1 | 0 | 1 | 1 处失水热区 |
| Test1.8 | 216 | 1 | 1 | 0 | 沿用原关卡，无陷阱 |
| Test1.9 | 420 | 1 | 3 | 0 | 沿用原关卡，无陷阱 |

- 道路按各关原有 Tilemap 占格生成，薄悬台、路面压边、侧墙与砖体分别选材。原地形继续承担碰撞；新表现位于原 Tilemap 子层级 `Ruins Terrain/Roads and Masonry`，调整路线时需同时更新两层。
- 水盆支柱按各自位置寻找下方原道路；电梯台面按各自碰撞尺寸布置，保留端点、速度及质量门槛。陶罐沿用空窗、蓝色满水窗和金色飞行水窗。
- 致死熔岩改用独立裁切的 Gothicvania 熔岩动画，填满原触发区的高度；顶刺使用 Magic Castle 的金属倒刺图片，代替原岩石样式。表现子层级名为 `Hazard Presentation`，不增加碰撞。
- 第 6、7 关 `Hot` 的作用是清空人物水量，沿用 `Empty` 标签，并以暖色热汽和轻微范围提示显示，区别于致死熔岩。表现位于 `Heat Zone Presentation`。不改动原触发效果。
- 陷阱和出口恢复到背景前方显示；尖刺及熔岩位于 MID 层 8，人物位于 HIGH 层。危险区域附近的本次新增火把、草丛已停用，减少误认。
- 尖刺 Sprite 位于 `Assets/Data/Presentation/Hazards/`，熔岩动画和控制器位于 `Assets/Animations/Hazards/`；复用原始图片，未修改源素材导入设置、共享陷阱预制体或玩法脚本。
- 修复死亡和出口仍引用 100 PPU 帧的问题：新增 `Assets/Animations/VolcanicTraveler/Hurt.anim`、`Exit.anim` 并绑定共享控制器。六个状态全部为 32 PPU、82×60 画布和 (41,30) 中心点，无缩放曲线。动作姿势仍会变化，贴图单位与物体缩放保持一致。
- 8 关共 277 项运行检查通过：六个动画状态的缩放和 PPU，空水/满水/飞行颜色及光点，水源补水和水纹，4 个陶罐三态，以及 7 台电梯数字刷新。另通过真实 `OnTriggerStay2D` 入口确认死亡进入 Hurt、出口进入 Exit，二者均不改变比例；热区清空水量且不会触发死亡。
- 保存后重新加载全部 8 关，原碰撞数据、地形碰撞轮廓、物理对象变换和玩法参数与修改前一致；缺失脚本为 0，道路表现 Tile 无缺图，角色引用有效。未完成每关人工通关，不能替代手感与路线验收。
- 预览：`Logs/VisualRefresh/chapter-N-after.png`（N 为上述关卡编号）；六态尺寸对比：`Logs/VisualRefresh/player-all-states.png`。验证汇总位于 `Logs/VisualRefresh/validation/chapter-rollout-summary.json`。
- 备份：`Logs/RefactorBackups/before-chapter-one-rollout-20260914.zip`（8 个场景和说明）；`before-hazard-and-player-states-20260914.zip`（陷阱与死亡比例修正前的场景、控制器和说明）。

## 烘干区动态遮罩（2026-09-14）

- 本节替代第 6、7 关原来的红色矩形和重复烟柱方案。最终以连续的半透明高温遮罩标识范围，取消本轮尝试的禁水标牌；焦黑路面、熔岩裂隙和喷口蒸汽作为环境辅助。
- `Hot/Geothermal Drying Zone/Continuous Heat Mask` 的网格直接来自原 `CompositeCollider2D.CreateMesh`，边界线使用原碰撞路径。保留台阶、凹口和分离小区域，不将外接矩形误当作完整烘干范围。
- `Assets/Shaders/Presentation/DryingHeatMask.shader` 使用流动热纹、连续暖色雾层及轻微背景折射表现热气，效果由时间驱动。材质和每关覆盖网格位于 `Assets/Data/Presentation/DryingZone/`。
- 遮罩位于 MID 9、轮廓位于 MID 10，人物继续位于 HIGH，保持可见。新表现物体没有 Collider2D；烘干触发形状、清空水量规则、道路碰撞、电梯及水源逻辑保持原样。
- 周围道路仅替换表现 Tile：第 6 关 98 格、第 7 关 82 格；新 Tile 位于 `Assets/Tilemap/DryingZone/`，发光裂隙裁切自 Gothicvania Volcanic Area 现有素材。未修改原始图片。
- 第 7 关原水盆在实际烘干触发区下方，仍能在原有位置取水；原大矩形覆盖曾把这处位置也罩住。本轮不扩大烘干触发范围。
- 新预览：`Logs/VisualRefresh/chapter-6-heat-mask.png`、`chapter-7-heat-mask.png`。备份为 `Logs/RefactorBackups/before-drying-zone-refresh-20260914.zip`，保存两关场景和本说明的修改前版本。
- 保存并重新加载后，两关遮罩网格与原触发网格一致，物理数据未改变、缺失脚本为 0、Shader 编译错误为 0；第 7 关运行检查确认烘干不致死且人物比例不变。自动渲染帧对比未验证出时间变化，流动效果仍需 Play 模式人工确认。结果记录于 `Logs/VisualRefresh/validation/drying-zone-summary.json`。

## 第二章：恢复原场景，保留人物和交互表现（2026-09-14）

- 根据用户最新要求，`Test2.1`～`Test2.7` 的地形、背景及新增环境装饰已回退。`Forest Terrain` 和 `Forest Atmosphere` 已从七关移除，原地形与背景的显示状态、排序及颜色从修改前备份恢复。
- 保留人物的 Traveler 动画、状态光点及倒水表现；陶罐的空水/满水/飞行状态显示；普通水源、金色净化水、水域、尖刺和出口的表现；第 7 关电梯台面、悬链及实时质量牌。
- 7 个角色、8 处净化水、2 个陶罐、1 处普通水源、1 台电梯及原有水域和陷阱保持交互表现绑定。原移动、碰撞、质量、取水及飞行规则未改变。
- 重新加载七关后，地形和背景 Tile、格子变换及格子颜色与原备份一致；物理及玩法快照未改变，缺失脚本为 0，角色与陶罐表现引用有效。本次未进行逐关人工通关。
- 修改前备份：`Logs/RefactorBackups/before-chapter-two-refresh-20260914.zip`；选择性回退前备份：`Logs/RefactorBackups/before-chapter-two-scenery-rollback-20260914.zip`。验证记录：`Logs/VisualRefresh/validation/chapter2-selective-rollback.json`。
- 回退后预览：`Logs/VisualRefresh/chapter2-1-scenery-restored.png`、`chapter2-7-scenery-restored.png`；此前 `chapter2-N-after.png` 是已撤销的地形方案，不代表当前场景。

## 第二章：陷阱、水域与人物状态特效（2026-09-14）

- 本轮仅更新第二章交互表现，保留回退后的原地形和背景。七关人物均绑定新增状态特效；未绑定这些可选引用的其他章节保持原表现。
- 第 2～6 关以 Cemetery 枯木荆棘图片替换密集金属尖刺，配合暗色木质核心。地面向上、顶部向下、竖直柱体两侧朝外；第 3 关有 20 组侧向荆棘。原陷阱判定未变，核心覆盖网格与原 CompositeCollider2D 网格一致。
- 第 4～6 关将活动水域合并成一个表现网格，加入半透明水流、疏密错开的上升气泡及外边界。合并处不重复叠色，也不画内部边界；覆盖范围依据原 water1 碰撞范围，不改变水下补水或飞行规则。
- `PlayerAnimations` 增加可选的 `fullWaterAura`、`flightAura`、`flightStream` 表现引用。满水显示蓝色水环及水滴；获得飞行状态显示金色气流翼；`IsFlying` 为真时额外播放向下的上升拖尾。倒水、补水、死亡、终点及禁用表现组件时按当前状态切换或清除特效，不改变人物缩放。
- 资源位于 `Assets/Data/Presentation/ChapterTwoFeedback/`，水域 Shader 为 `Assets/Shaders/Presentation/UnderwaterRegion.shader`；复用项目内图片，没有修改原图导入设置。
- 16 项人物运行检查通过，涵盖取水、倒水、飞行准备/起飞、重新装水、禁用/重新启用、死亡及六种动画状态的比例与 32 PPU；水域补水、离开后保留满水提示也已验证。水域运行帧对比确认动画变化。
- 保存并重载七关后，角色引用有效、缺失脚本为 0、没有新增碰撞；陷阱核心与原碰撞网格一致，水域合并覆盖面积一致，原物理及玩法快照不变。检查结束时控制台无错误或警告；尚未逐关人工通关。
- 预览：`Logs/VisualRefresh/chapter2-3-interaction-feedback.png`、`chapter2-5-interaction-feedback.png`、`player-feedback-full.png`、`player-feedback-flight-ready.png`、`player-feedback-flying.png`。记录：`Logs/VisualRefresh/validation/chapter2-interaction-feedback.json`。
- 备份：`Logs/RefactorBackups/before-chapter-two-interaction-feedback-20260914.zip`，包含七关场景、PlayerAnimations 和本说明的修改前版本。

## 第三章：人物、物品与交互范围（2026-09-21）

- 覆盖 `Assets/Chapter three/Test3.1.unity`～`Test3.5.unity`。使用项目实际环境 Unity 2022.3.62f3 / Built-in；原有地形、背景、陷阱 Tilemap、平台和电梯外观、端点、质量及碰撞保留。
- 五个人物绑定 `Traveler Presentation`，沿用前两章的六态动画、落地/倒水粒子、满水水环和飞行表现。原根物体 Animator 和 SpriteRenderer 在场景实例上停用，根物体缩放与物理组件保持不变。
- 五处水源使用动态石质水盆，一处陶罐使用空窗/蓝色满水窗/金色飞行水窗。显示层与原交互范围对齐，不增加碰撞。
- 五处寒冷区使用淡蓝雾层、飘雪及连续边界，覆盖网格来自原 BoxCollider2D；第 1、3、5 关的三处有效热区沿用第一章热气遮罩，网格来自原 CompositeCollider2D。第 4 关没有有效热区，未凭空添加。区域表现位于 MID，人物位于 HIGH。
- `PlayerAnimations.coldWaterAura` 是新增的可选 `[SerializeField]` 引用，Inspector 可保存，在第三章的场景实例上绑定。仅在 `IceBlock && Full && !IsStopped` 时显示雪花；倒水、离开寒冷区、死亡或禁用表现时隐藏。前两章没有绑定该引用，保持原表现。
- 第三章人物改为引用专用 `Ice Block.prefab`，以方形冰晶棱面、裂纹和生成闪点表现冰块。原 `IceBlock.prefab` 未修改；新预制体保留其 Ice、Rigidbody2D、BoxCollider2D、标签和层配置，质量、尺寸、生成规则及受热销毁规则不变。
- 表现预制体：`Assets/Prefabs/Presentation/ChapterThree/`；遮罩网格、材质及冰块网格：`Assets/Data/Presentation/ChapterThree/`；寒冷区 Shader：`Assets/Shaders/Presentation/ColdRegion.shader`。
- 保存并重新加载五关后，地形、平台、原碰撞和玩法参数快照一致（人物 iceBlockPrefab 引用按本次外观变体替换）；缺失脚本为 0，人物九项表现引用有效，新增表现无 Collider，区域 Shader 无编译错误。
- 第 3 关通过 21 项运行检查：冷区空水/补水提示、实际 Release 生成冰块、冰块质量和碰撞尺寸、离开寒冷区、热区失水、六态缩放与 32 PPU、飞行反馈、禁用/恢复/死亡清理，以及陶罐三种水位显示。尚未完成五关人工通关。
- 预览：`Logs/VisualRefresh/chapter3-N-interactions.png`（N=1～5）；`chapter3-player-ice-feedback.png` 为运行检查特写，其中生成的冰块仅在运行时移到人物右侧便于对照。验证记录位于 `Logs/VisualRefresh/validation/chapter3-*.json`。
- 修改前备份：`Logs/RefactorBackups/before-chapter-three-interactions-20260921.zip`，包含五关、PlayerAnimations 和本说明。

## 冰块融化、堆叠稳定与间接承重修复（2026-09-21）

- `Ice` 保留首次接触 Empty 热区后 3 秒销毁的规则，期间同步降低冰面、边框和裂纹透明度；离开热区仍继续融化，再次接触不会重置倒计时。`meltDuration` 使用 `[SerializeField]`，Inspector 可调整。使用 MaterialPropertyBlock，不修改共享材质；不支持颜色属性的旧粒子材质由粒子自身短寿命收尾。
- `PlayerState.TryCreateIceBlock` 按人物与冰块碰撞尺寸计算前方生成位置，并检测实际生成空间。冰块或墙壁占用该位置时不生成、不消耗水量；避免旧的 0.2 单位中心距离导致人物与冰块重叠。原冰块碰撞尺寸和质量未缩小。
- `Floor` 改用 Kinematic Rigidbody2D，在 FixedUpdate 中 MovePosition；未配置刚体的旧场景在 Awake 自动补充。取消动态承重物体与平台之间的 SetParent，避免 Transform 和刚体同时驱动物体。平台外观、端点、速度和门槛配置不变。
- 质量统计沿向上的实际碰撞接触逐层遍历，包含人物、陶罐与上层冰块；每个 IMassProvider 只计一次，侧面接触不算承托。融化销毁或物体离开后实时刷新质量牌；继续兼容陶罐子层级中携带的人物。
- 第 3.4 关实际物理检查：两块底座与三块上层显示 5/10，稳定后的 5 秒采样位移及速度均为 0；平台移动 1.8 单位时质量始终为 5，冰块相对位移最大约 0.00003 单位。移除上层桥接冰块后为 4；上层站空水/满水人物分别为 6/10、10/10；侧面接触不加质量。
- 真实时间融化检查：约 1.55 秒透明度为 48%，3.3 秒采样时已销毁，质量由 5/10 变为 4/10。材质兼容修正后另以新旧冰块与陶罐复查，空罐加两块冰为 3、满罐加两块冰为 7，淡出及销毁刷新再次通过。左右生成、重复生成和墙壁阻挡检查通过。未逐关人工通关。
- 检查记录：`Logs/VisualRefresh/validation/ice-*.json`；预览：`Logs/VisualRefresh/ice-stack-mass-fixed.png`、`ice-melt-stage-1.png`。运行检查生成的临时冰块和热区未保存进场景。
- 修改前备份：`Logs/RefactorBackups/before-ice-melting-stack-mass-20260921.zip`。本轮修改 Ice、PlayerState、Floor 三个脚本及本说明。

## 电梯启动造成冰块抽搐的补充修复（2026-09-21）

- 在用户实际运行的 Test3.5 复现质量 0/5 与 5/5 反复切换，平台在起点附近往返。该关速度为 1.5，旧的恒速 MovePosition 在满载时瞬间向下启动；冰块来不及跟随，失去接触后平台立即反向返回。上一轮速度 0.5 的分步物理检查未覆盖此情况。
- 仅调整 `Floor.FixedUpdate` 的运动速度：启动及反向时逐渐加速，接近端点时依据剩余制动距离减速。加速度不超过重力的一半，也不超过 10 单位/秒²，保持向下启动、向上刹停时的承托接触。原 speed 仍为速度上限，端点、质量门槛、碰撞尺寸及平台外观不变；未延迟或伪造质量读数。
- Test3.5 正常 Play 模式连续检查 12 秒：原速度 1.5、原门槛 5，五块冰块下行到终点全过程质量保持 5；真实热区使一块融化后质量变为 4，余下冰块随平台返回。最后 1.2 秒平台位置始终为 -2.49，冰块速度均为 0。检查同时保持 Animator、插值和默认物理更新运行。
- 修复前后运行轨迹：`Logs/VisualRefresh/validation/ice-jitter-real-before.json`、`ice-jitter-real-after.json`；双层堆叠记录：`ice-jitter-two-layers-after.json`。未逐关人工通关。
- 备份：`Logs/RefactorBackups/before-ice-platform-jitter-20260921.zip`。本轮仅修改 Floor 脚本和本说明，运行检查物体不保存进场景。
