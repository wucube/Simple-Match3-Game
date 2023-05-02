### 功能实现
一个方块有两个及以上的同类型相邻方块时，点击该方块就能消除。

点击消除方块增长技能槽，点击方块没有消除扣除技能槽。

技能槽满后随机出现两种技能方块，点击技能方块会消除四周的方块或同行同列的所有方块。

每轮方块的消除时长为1分钟，时间到达后自动结束游戏。

游戏暂停时不会倒计时，结束暂停继续倒计时。

方块消除后自动随机新境方块，并自动填补方块空缺的位置。

每次生成的方块都至少有一次消除机会，不会发生新生成方块后无法消除的情况。

使用PlayerPrefs实现玩家数据的持久化，记录玩家上次游玩时间和最高分数记录。

### 运行环境
Unity 2021.3.23f1c1(LTS)

![Image txet](https://github.com/wucube/Simple-Match3-Game/blob/main/_Image/%E5%BC%80%E5%A7%8B%E8%8F%9C%E5%8D%95.jpg)

![Image txet](https://github.com/wucube/Simple-Match3-Game/blob/main/_Image/%E6%B8%B8%E7%8E%A9%E7%95%8C%E9%9D%A2.jpg)