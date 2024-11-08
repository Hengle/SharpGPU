using System;
using Infinity.Core;
using Infinity.Mathmatics;

namespace Infinity.Graphics
{
    public struct RHIIndirectDispatchArgs
    {
        public uint GroupCountX;
        public uint GroupCountY;
        public uint GroupCountZ;
        public RHIIndirectDispatchArgs(in uint groupCountX, in uint groupCountY, in uint groupCountZ)
        {
            GroupCountX = groupCountX;
            GroupCountY = groupCountY;
            GroupCountZ = groupCountZ;
        }
    }

    public struct RHIIndirectDrawArgs
    {
        public uint VertexCount;
        public uint InstanceCount;
        public uint StartVertexLocation;
        public uint StartInstanceLocation;
        public RHIIndirectDrawArgs(in uint vertexCount, in uint instanceCount, in uint startVertexLocation, in uint startInstanceLocation)
        {
            VertexCount = vertexCount;
            InstanceCount = instanceCount;
            StartVertexLocation = startVertexLocation;
            StartInstanceLocation = startInstanceLocation;
        }
    }

    public struct RHIIndirectDrawIndexedArgs
    {
        public uint IndexCount;
        public uint InstanceCount;
        public uint StartIndexLocation;
        public int BaseVertexLocation;
        public uint StartInstanceLocation;
        public RHIIndirectDrawIndexedArgs(in uint indexCount, in uint instanceCount, in uint startIndexLocation, in int baseVertexLocation, in uint startInstanceLocation)
        {
            IndexCount = indexCount;
            InstanceCount = instanceCount;
            StartIndexLocation = startIndexLocation;
            BaseVertexLocation = baseVertexLocation;
            StartInstanceLocation = startInstanceLocation;
        }
    }

    public struct RHIBufferCopyDescriptor
    {
        public uint Offset;
        public uint RowPitch;
        public uint3 TextureHeight;
        public RHIBuffer Buffer;
    }

    public struct RHITextureCopyDescriptor
    {
        public uint MipLevel;
        public uint SliceBase;
        public uint SliceCount;
        public uint3 Origin;
        public RHITexture Texture;
    }

    public struct RHITimestampDescriptor
    {
        public RHIQuery Query;
        public uint BeginIndex;
        public uint EndIndex;
    }

    public struct RHIOcclusionDescriptor
    {
        public RHIQuery Query;
    }

    public struct RHIStatisticsDescriptor
    {
        public RHIQuery Query;
        public uint WriteIndex;
    }

    public struct RHIColorAttachmentDescriptor
    {
        public uint MipLevel;
        public uint ArraySlice;
        public float4 ClearValue;
        public ERHILoadAction LoadAction;
        public ERHIStoreAction StoreAction;
        public RHITexture RenderTarget;
        public uint ResolveMipLevel;
        public uint ResolveArraySlice;
        public RHITexture ResolveTarget;
    }

    public struct RHIDepthStencilAttachmentDescriptor
    {
        public uint MipLevel;
        public uint ArraySlice;
        public float DepthClearValue;
        public ERHILoadAction DepthLoadOp;
        public ERHIStoreAction DepthStoreOp;
        public int StencilClearValue;
        public ERHILoadAction StencilLoadOp;
        public ERHIStoreAction StencilStoreOp;
        public RHITexture RenderTarget;
        public uint ResolveMipLevel;
        public uint ResolveArraySlice;
        public EResolveMode ResolveMode;
        public RHITexture ResolveTarget;
    }

    public struct RHIAttachmentIndexArray
    {
        public static RHIAttachmentIndexArray Emtpy = new RHIAttachmentIndexArray(0);

        public const int MaxAttachments = 8;

        private int a0;

        private int a1;

        private int a2;

        private int a3;

        private int a4;

        private int a5;

        private int a6;

        private int a7;

        private int activeAttachments;

        public int Length => activeAttachments;

        public unsafe int this[int index]
        {
            get
            {
                if ((uint)index >= 8u)
                {
                    throw new IndexOutOfRangeException($"AttachmentIndexArray - index must be in range of [0, {8}[");
                }

                if ((uint)index >= activeAttachments)
                {
                    throw new IndexOutOfRangeException($"AttachmentIndexArray - index must be in range of [0, {activeAttachments}[");
                }

                fixed (RHIAttachmentIndexArray* ptr = &this)
                {
                    int* ptr2 = (int*)ptr;
                    return ptr2[index];
                }
            }
            set
            {
                if ((uint)index >= 8u)
                {
                    throw new IndexOutOfRangeException($"AttachmentIndexArray - index must be in range of [0, {8}[");
                }

                if ((uint)index >= activeAttachments)
                {
                    throw new IndexOutOfRangeException($"AttachmentIndexArray - index must be in range of [0, {activeAttachments}[");
                }

                fixed (RHIAttachmentIndexArray* ptr = &this)
                {
                    int* ptr2 = (int*)ptr;
                    ptr2[index] = value;
                }
            }
        }

        public RHIAttachmentIndexArray(in int numAttachments)
        {
            if (numAttachments < 0 || numAttachments > 8)
            {
                throw new ArgumentException($"AttachmentIndexArray - numAttachments must be in range of [0, {8}[");
            }

            a0 = (a1 = (a2 = (a3 = (a4 = (a5 = (a6 = (a7 = -1)))))));
            activeAttachments = numAttachments;
        }

        public RHIAttachmentIndexArray(int[] attachments) : this(attachments.Length)
        {
            for (int i = 0; i < activeAttachments; i++)
            {
                this[i] = attachments[i];
            }
        }
    }

    public struct RHISubPassDescriptor
    {
        public ERHISubPassFlags Flags;
        public RHIAttachmentIndexArray ColorInputs;
        public RHIAttachmentIndexArray ColorOutputs;
    }

    public struct RHITransferPassDescriptor
    {
        public string Name;
        public RHITimestampDescriptor? Timestamp;
    }

    public struct RHIComputePassDescriptor
    {
        public string Name;
        public RHITimestampDescriptor? Timestamp;
        public RHIStatisticsDescriptor? Statistics;
    }

    public struct RHIRayTracingPassDescriptor
    {
        public string Name;
        public RHITimestampDescriptor? Timestamp;
        public RHIStatisticsDescriptor? Statistics;
    }

    public struct RHIRasterPassDescriptor
    {
        public string Name;
        public uint ArrayLength;
        public ERHISampleCount SampleCount;
        public RHITimestampDescriptor? Timestamp;
        public RHIOcclusionDescriptor? Occlusion;
        public RHIStatisticsDescriptor? Statistics;
        public RHITexture ShadingRateTexture;
        public Memory<RHIColorAttachmentDescriptor> ColorAttachments;
        public RHIDepthStencilAttachmentDescriptor? DepthStencilAttachment;
        public Memory<RHISubPassDescriptor> SubPassDescriptors;
    }

    public abstract class RHITransferEncoder : Disposal
    {
        protected RHICommandBuffer? m_CommandBuffer;

        internal abstract void BeginPass(in RHITransferPassDescriptor descriptor);
        public abstract void PushDebugGroup(string name);
        public abstract void PopDebugGroup();
        public abstract void WriteTimestamp(in uint index);
        public abstract void ResolveQuery(RHIQuery query, in uint startIndex, in uint queriesCount);
        public abstract void CopyBufferToBuffer(RHIBuffer srcBuffer, in int srcOffset, RHIBuffer dstBuffer, in int dstOffset, in int size);
        public abstract void CopyBufferToTexture(in RHIBufferCopyDescriptor src, in RHITextureCopyDescriptor dst, in int3 size);
        public abstract void CopyTextureToBuffer(in RHITextureCopyDescriptor src, in RHIBufferCopyDescriptor dst, in int3 size);
        public abstract void CopyTextureToTexture(in RHITextureCopyDescriptor src, in RHITextureCopyDescriptor dst, in int3 size);
        public abstract void EndPass();
    }

    public abstract class RHIComputeEncoder : Disposal
    {
        protected RHICommandBuffer? m_CommandBuffer;
        protected RHIComputePipeline? m_CachedPipeline;

        internal abstract void BeginPass(in RHIComputePassDescriptor descriptor);
        public abstract void PushDebugGroup(string name);
        public abstract void PopDebugGroup();
        public abstract void WriteTimestamp(in uint index);
        public abstract void BeginStatistics(in uint index);
        public abstract void EndStatistics(in uint index);
        public abstract void MemoryBarrier(RHIBuffer buffer, in ERHIBufferState srcState, in ERHIBufferState dstState);
        public abstract void MemoryBarrier(RHITexture texture, in ERHITextureState srcState, in ERHITextureState dstState);
        public abstract void SetPipeline(RHIComputePipeline pipeline);
        public abstract void SetResourceTable(RHIResourceTable resourceTable, in uint tableIndex);
        public abstract void Dispatch(in uint groupCountX, in uint groupCountY, in uint groupCountZ);
        public abstract void DispatchIndirect(RHIBuffer argsBuffer, in uint argsOffset);
        public abstract void ExecuteIndirectCommandBuffer(RHIComputeIndirectCommandBuffer indirectCmdBuffer);
        public abstract void EndPass();
    }

    public abstract class RHIRaytracingEncoder : Disposal
    {
        protected RHICommandBuffer? m_CommandBuffer;
        protected RHIRaytracingPipeline? m_CachedPipeline;

        internal abstract void BeginPass(in RHIRayTracingPassDescriptor descriptor);
        public abstract void PushDebugGroup(string name);
        public abstract void PopDebugGroup();
        public abstract void WriteTimestamp(in uint index);
        public abstract void BeginStatistics(in uint index);
        public abstract void EndStatistics(in uint index);
        public abstract void MemoryBarrier(RHIBuffer buffer, in ERHIBufferState srcState, in ERHIBufferState dstState);
        public abstract void MemoryBarrier(RHITexture texture, in ERHITextureState srcState, in ERHITextureState dstState);
        public abstract void SetPipeline(RHIRaytracingPipeline pipeline);
        public abstract void SetResourceTable(RHIResourceTable resourceTable, in uint tableIndex);
        public abstract void BuildAccelerationStructure(RHITopLevelAccelStruct topLevelAccelStruct);
        public abstract void BuildAccelerationStructure(RHIBottomLevelAccelStruct bottomLevelAccelStruct);
        public abstract void Dispatch(in uint width, in uint height, in uint depth, RHIFunctionTable functionTable);
        public abstract void DispatchIndirect(RHIBuffer argsBuffer, in uint argsOffset, RHIFunctionTable functionTable);
        public abstract void ExecuteIndirectCommandBuffer(RHIRayTracingIndirectCommandBuffer indirectCmdBuffer);
        public abstract void EndPass();
    }
    
    public abstract class RHIRasterEncoder : Disposal
    {
        protected RHICommandBuffer? m_CommandBuffer;
        protected RHIRasterPipeline? m_CachedPipeline;

        internal abstract void BeginPass(in RHIRasterPassDescriptor descriptor);
        public abstract void PushDebugGroup(string name);
        public abstract void PopDebugGroup();
        public abstract void WriteTimestamp(in uint index);
        public abstract void BeginOcclusion(in uint index);
        public abstract void EndOcclusion(in uint index);
        public abstract void BeginStatistics(in uint index);
        public abstract void EndStatistics(in uint index);
        public abstract void NextSubPass();
        public abstract void SetScissor(in Rect rect);
        public abstract void SetScissors(in Memory<Rect> rects);
        public abstract void SetViewport(in Viewport viewport);
        public abstract void SetViewports(in Memory<Viewport> viewports);
        public abstract void SetStencilRef(in uint value);
        public abstract void SetBlendFactor(in float4 value);
        public abstract void SetPipeline(RHIRasterPipeline pipeline);
        public abstract void SetResourceTable(RHIResourceTable resourceTable, in uint tableIndex);
        public abstract void SetIndexBuffer(RHIBuffer buffer, in uint offset);
        public abstract void SetVertexBuffer(RHIBuffer buffer, in uint slot, in uint offset);
        public abstract void SetShadingRate(in ERHIShadingRate shadingRate, in ERHIShadingRateCombiner shadingRateCombiner);
        public abstract void Draw(in uint vertexCount, in uint instanceCount, in uint firstVertex, in uint firstInstance);
        public abstract void DrawIndexed(in uint indexCount, in uint instanceCount, in uint firstIndex, in uint baseVertex, in uint firstInstance);
        public abstract void DrawIndirect(RHIBuffer argsBuffer, in uint offset, in uint drawCount);
        public abstract void DrawIndexedIndirect(RHIBuffer argsBuffer, in uint offset, in uint drawCount);
        public abstract void DrawMesh(in uint groupCountX, in uint groupCountY, in uint groupCountZ);
        public abstract void DrawMeshIndirect(RHIBuffer argsBuffer, in uint argsOffset);
        public abstract void ExecuteIndirectCommandBuffer(RHIRasterIndirectCommandBuffer indirectCmdBuffer);
        public abstract void EndPass();
    }
}
