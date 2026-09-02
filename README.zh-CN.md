<div align="center">

# FlatMC Unity Edition

### FlatMC 2D 沙盒游戏的 Unity 历史实现

[![Unity](https://img.shields.io/badge/Unity-2022.3-black?logo=unity)](https://unity.com/)
[![Version](https://img.shields.io/badge/Version-0.2.0-blue)](#版本记录)
[![License](https://img.shields.io/badge/License-MIT-blue)](LICENSE)
[![Status](https://img.shields.io/badge/Status-Archived-lightgrey)](#项目状态)

[English](README.md) | **简体中文**

</div>

> [!IMPORTANT]
> FlatMC Unity Alpha 已停止开发和维护，FlatMC 已由 **FlatCraft** 取代。本仓库仅作为历史记录保留，不保证与 FlatCraft 兼容。

## 项目简介

FlatMC Unity Alpha 是面向移动端重制的 Unity 实现，加入了触屏操作、背景层、水、光照、存储、熔炼、饥饿、盔甲和 3D 玩家模型。项目随后转向 Godot 开发。

## 版本记录

| 版本 | 原版本 | 日期 | 主要内容 |
| --- | --- | --- | --- |
| v0.1.0 | Unity v0.5.0-alpha | 2024-03-04 | 首个 Android 测试版本 |
| v0.2.0 | Unity v0.5.0-beta | 2024-05-23 | 玩家、生存、存储和熔炼更新；Unity 最终版本 |

首个规范化 GitHub Release 为 `v0.1.0`。后续版本将在确认后发布。

## v0.1.0 内容

- Android APK 和触屏摇杆操作。
- 背景墙、人物头部晃动、视差背景、昼夜优化和全新 UI。
- 沙子、水、空桶和水桶。
- 锤子、锄头、门、楼梯、梯子、装备、玻璃、苹果和耕地。
- 梯子攀爬、水中移动、取水倒水、工作台和门交互。

该早期版本尚未完成创造模式、僵尸、命令、多人游戏、食物、盔甲穿戴和完整死亡掉落等系统。

## 技术栈

- Unity 2022.3 LTS
- C#
- Universal Render Pipeline
- Android 与实验性桌面平台

## 已知限制

- 当前仓库是后期保存快照，不是还原后的 `v0.1.0` 源码树。
- 当前没有附带可验证的原始 `v0.1.0` APK。
- iOS 支持没有完成。
- 本项目不兼容 FlatCraft。

## 项目状态

**已归档 / 已被取代**

Unity 版在原 beta 版本后结束开发，FlatCraft 是 FlatMC 的后继项目。

## 许可证

本项目采用 [MIT License](LICENSE)。素材和第三方组件可能适用单独条款。
