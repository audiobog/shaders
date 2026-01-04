/*{
    "CREDIT": "Jason Beyers",

    "DESCRIPTION": "A flexible video wall effect with many animation options.\n\nTips:\n1. To display square tiles of 16:9 content on a 16:9 quad, set input aspect to 0.563 and UV aspect to 1.777.\n\n2. Tile borders get a little tricky with alpha.  Play around with the border-specific and global alpha settings to get the desired look.",

    "VSN": "1.2",

    "INPUTS": [

        {
            "LABEL": "Mix/Mix",
            "NAME": "fx_filter_mix",
            "TYPE": "float",
            "MIN": 0.0,
            "MAX": 1.0,
            "DEFAULT": 1.0
        },
        {
            "LABEL": "Mix/Add Orig",
            "NAME": "fx_mix_add",
            "TYPE": "float",
            "MIN": 0.0,
            "MAX": 1.0,
            "DEFAULT": 0.0
        },
        {
            "LABEL": "Mix/Add Mode",
            "NAME": "fx_mix_add_mode",
            "TYPE": "long",
            "VALUES": ["Sum","Max"], "DEFAULT": "Sum"
        },

        {
            "LABEL": "Input/Scale",
            "NAME": "fx_input_scale",
            "TYPE": "float",
            "MIN": 0.0,
            "MAX": 2.0,
            "DEFAULT": 1.0
        },
        {
            "LABEL": "Input/Aspect",
            "NAME": "fx_input_aspect",
            "TYPE": "float",
            "MIN": 0.0,
            "MAX": 4.0,
            "DEFAULT": 1.0
        },

        {
            "Label": "Input/Rotate",
            "NAME": "fx_input_rotate",
            "TYPE": "float",
            "MIN": -360.0,
            "MAX": 360.,
            "DEFAULT": 0.0
        },
        {
            "LABEL": "Input/Shift",
            "NAME": "fx_input_offset",
            "TYPE": "point2D",
            "MIN": [-1.0,-1.0],
            "MAX": [1.0,1.0],
            "DEFAULT": [0.0,0.0]
        },
        {
            "LABEL": "Input/Flip X",
            "NAME": "fx_input_flip_x",
            "TYPE": "bool",
            "DEFAULT": false,
            "FLAGS": "button"
        },
        {
            "LABEL": "Input/Flip Y",
            "NAME": "fx_input_flip_y",
            "TYPE": "bool",
            "DEFAULT": false,
            "FLAGS": "button"
        },


        {
            "LABEL": "Input/Mirror X",
            "NAME": "fx_input_mirror_x",
            "TYPE": "bool",
            "DEFAULT": false,
            "FLAGS": "button"
        },
        {
            "LABEL": "Input/Mirror Y",
            "NAME": "fx_input_mirror_y",
            "TYPE": "bool",
            "DEFAULT": false,
            "FLAGS": "button"
        },
        

        {
            "LABEL": "UV/Scale",
            "NAME": "fx_scale",
            "TYPE": "float",
            "MIN": 0.0,
            "MAX": 4.0,
            "DEFAULT": 1.0
        },
        {
            "LABEL": "UV/Aspect",
            "NAME": "fx_aspect",
            "TYPE": "float",
            "MIN": 0.0,
            "MAX": 4.0,
            "DEFAULT": 1.0
        },
        {
            "Label": "UV/Rotate",
            "NAME": "fx_rotate",
            "TYPE": "float",
            "MIN": -360.0,
            "MAX": 360.,
            "DEFAULT": 0.0
        },
        {
            "LABEL": "UV/Shift",
            "NAME": "fx_shift_amount",
            "TYPE": "point2D",
            "MIN": [-1.0,-1.0],
            "MAX": [1.0,1.0],
            "DEFAULT": [0.0,0.0]
        },
        {
            "LABEL": "UV/Shift Scale",
            "NAME": "fx_shift_scale",
            "TYPE": "float",
            "MIN": 0.0,
            "MAX": 4.0,
            "DEFAULT": 1.0
        },
        {
            "LABEL": "UV/Shift Type",
            "NAME": "fx_shift_type",
            "TYPE": "long",
            "VALUES": ["Pre Rotate","Post Rotate"],
            "DEFAULT": "Pre Rotate"
        },

        {
            "LABEL": "Tiles/Repeat",
            "NAME": "fx_repeats",
            "TYPE": "int",
            "MIN": 1,
            "MAX": 8,
            "DEFAULT": 2
        },
        {
            "LABEL": "Tiles/Mode",
            "NAME": "fx_tile_mode",
            "TYPE": "long",
            "VALUES": ["Basic","Flip X by column", "Flip Y by column", "Flip X by row", "Flip Y by row", "Flip X corner", "Flip Y corner"],
            "DEFAULT": "Basic"
        },
        {
            "LABEL": "Tiles/Shape",
            "NAME": "fx_tile_shape",
            "TYPE": "long",
            "VALUES": ["Square","Circle"],
            "DEFAULT": "Square"
        },
        {
            "LABEL": "Tiles/Tile Size",
            "NAME": "fx_tile_size",
            "TYPE": "float",
            "MIN": 0.0,
            "MAX": 1.0,
            "DEFAULT": 1.0
        },
        {
            "LABEL": "Tiles/Scale Tiles To Fit Region",
            "NAME": "fx_constrain_tiles",
            "TYPE": "bool",
            "DEFAULT": true,
            "FLAGS": "button"
        },

        {
            "LABEL": "Tiles/Background",
            "NAME": "fx_tile_back_color",
            "TYPE": "color",
            "DEFAULT": [
                0.0,
                0.0,
                0.0,
                0.0
            ]
        },

        {
            "LABEL": "Scroll/Animate",
            "NAME": "fx_shift_animate",
            "TYPE": "bool",
            "DEFAULT": true,
            "FLAGS": "button"
        },

        {
            "Label": "Scroll/Direction",
            "NAME": "fx_shift_angle",
            "TYPE": "float",
            "MIN": 0.0,
            "MAX": 360.0,
            "DEFAULT": 0.0
        },
        {
            "LABEL": "Scroll/Speed",
            "NAME": "fx_shift_speed",
            "TYPE": "float",
            "MIN": 0.0,
            "MAX": 4.0,
            "DEFAULT": 1.0
        },
        {
            "LABEL": "Scroll/BPM Sync",
            "NAME": "fx_shift_bpm_sync",
            "TYPE": "bool",
            "DEFAULT": false,
            "FLAGS": "button"
        },
        {
            "LABEL": "Scroll/Reverse",
            "NAME": "fx_shift_reverse",
            "TYPE": "bool",
            "DEFAULT": 0,
            "FLAGS": "button"
        },
        {
            "Label": "Scroll/Offset",
            "NAME": "fx_shift_offset",
            "TYPE": "float",
            "MIN": 0.0,
            "MAX": 1.0,
            "DEFAULT": 0.0
        },
        {
            "Label": "Scroll/Offset Scale",
            "NAME": "fx_shift_offset_scale",
            "TYPE": "float",
            "MIN": 0.0,
            "MAX": 4.0,
            "DEFAULT": 1.0
        },
        {
            "Label": "Scroll/Strobe",
            "NAME": "fx_shift_strob",
            "TYPE": "float",
            "MIN": 0.0,
            "MAX": 1.0,
            "DEFAULT": 0.0
        },
        {
            "LABEL": "Scroll/Restart",
            "NAME": "fx_shift_restart",
            "TYPE": "event",
        },

        {
            "LABEL": "Wobble/Animate",
            "NAME": "fx_wobble_animate",
            "TYPE": "bool",
            "DEFAULT": true,
            "FLAGS": "button"
        },
        {
            "LABEL": "Wobble/Type",
            "NAME": "fx_wobble_mode",
            "TYPE": "long",
            "VALUES": ["Circular","Noise 1","Noise 2"],
            "DEFAULT": "Circular"
        },
        {
            "Label": "Wobble/Range",
            "NAME": "fx_wobble_range",
            "TYPE": "float",
            "MIN": 0.0,
            "MAX": 1.0,
            "DEFAULT": 0.1
        },
        {
            "LABEL": "Wobble/Speed",
            "NAME": "fx_wobble_speed",
            "TYPE": "float",
            "MIN": 0.0,
            "MAX": 4.0,
            "DEFAULT": 0.5
        },
        {
            "LABEL": "Wobble/BPM Sync",
            "NAME": "fx_wobble_bpm_sync",
            "TYPE": "bool",
            "DEFAULT": false,
            "FLAGS": "button"
        },
        {
            "LABEL": "Wobble/Reverse",
            "NAME": "fx_wobble_reverse",
            "TYPE": "bool",
            "DEFAULT": 0,
            "FLAGS": "button"
        },
        {
            "Label": "Wobble/Offset",
            "NAME": "fx_wobble_offset",
            "TYPE": "float",
            "MIN": 0.0,
            "MAX": 1.0,
            "DEFAULT": 0.0
        },
        {
            "Label": "Wobble/Offset Scale",
            "NAME": "fx_wobble_offset_scale",
            "TYPE": "float",
            "MIN": 0.0,
            "MAX": 4.0,
            "DEFAULT": 1.0
        },
        {
            "Label": "Wobble/Strobe",
            "NAME": "fx_wobble_strob",
            "TYPE": "float",
            "MIN": 0.0,
            "MAX": 1.0,
            "DEFAULT": 0.0
        },
        {
            "LABEL": "Wobble/Restart",
            "NAME": "fx_wobble_restart",
            "TYPE": "event",
        },

        {
            "LABEL": "Rotate/Animate",
            "NAME": "fx_animate_rotate",
            "TYPE": "bool",
            "DEFAULT": false,
            "FLAGS": "button"
        },
        {
            "LABEL": "Rotate/Range",
            "NAME": "fx_rotate_range",
            "TYPE": "floatRange",
            "DEFAULT": [0.0,360.0],
            "MIN": -360.0,
            "MAX": 360.0
        },
        {
            "LABEL": "Rotate/Signal",
            "NAME": "fx_rotate_signal",
            "TYPE": "long",
            "VALUES": ["Saw","Inverse Saw","Square","Inverse Square","Triangle","Sine"],
            "DEFAULT": "Saw"
        },

        {
            "LABEL": "Rotate/Filter",
            "NAME": "fx_rotate_filter",
            "TYPE": "long",
            "VALUES": ["Ease In","Ease Out","Ease In Out","Ease Out In"],
            "DEFAULT": "Ease In Out"
        },
        {
            "LABEL": "Rotate/Curve",
            "NAME": "fx_rotate_curve",
            "TYPE": "float",
            "MIN": 1.0,
            "MAX": 8.0,
            "DEFAULT": 1.0
        },
        {
            "LABEL": "Rotate/Speed",
            "NAME": "fx_rotate_speed",
            "TYPE": "float",
            "MIN": 0.0,
            "MAX": 4.0,
            "DEFAULT": 0.5
        },

        {
            "LABEL": "Rotate/BPM Sync",
            "NAME": "fx_rotate_bpm_sync",
            "TYPE": "bool",
            "DEFAULT": false,
            "FLAGS": "button"
        },
        {
            "LABEL": "Rotate/Reverse",
            "NAME": "fx_rotate_reverse",
            "TYPE": "bool",
            "DEFAULT": 0,
            "FLAGS": "button"
        },
        {
            "Label": "Rotate/Offset",
            "NAME": "fx_rotate_offset",
            "TYPE": "float",
            "MIN": 0.0,
            "MAX": 1.0,
            "DEFAULT": 0.0
        },
        {
            "Label": "Rotate/Strobe",
            "NAME": "fx_rotate_strob",
            "TYPE": "float",
            "MIN": 0.0,
            "MAX": 1.0,
            "DEFAULT": 0.0
        },
        {
            "LABEL": "Rotate/Restart",
            "NAME": "fx_rotate_restart",
            "TYPE": "event",

        },

        {
            "LABEL": "Scale/Animate",
            "NAME": "fx_animate_scale",
            "TYPE": "bool",
            "DEFAULT": false,
            "FLAGS": "button"
        },
        {
            "LABEL": "Scale/Mode",
            "NAME": "fx_scale_mode",
            "TYPE": "long",
            "VALUES": ["Add","Subtract"],
            "DEFAULT": "Add"
        },
        {
            "LABEL": "Scale/Range",
            "NAME": "fx_scale_range",
            "TYPE": "floatRange",
            "DEFAULT": [0.0,1.0],
            "MIN": 0.0,
            "MAX": 1.0
        },
        {
            "LABEL": "Scale/Signal",
            "NAME": "fx_scale_signal",
            "TYPE": "long",
            "VALUES": ["Saw","Inverse Saw","Square","Inverse Square","Triangle","Sine"],
            "DEFAULT": "Sine"
        },
        {
            "LABEL": "Scale/Filter",
            "NAME": "fx_scale_filter",
            "TYPE": "long",
            "VALUES": ["Ease In","Ease Out","Ease In Out","Ease Out In"],
            "DEFAULT": "Ease In Out"
        },
        {
            "LABEL": "Scale/Curve",
            "NAME": "fx_scale_curve",
            "TYPE": "float",
            "MIN": 1.0,
            "MAX": 8.0,
            "DEFAULT": 1.0
        },


        {
            "LABEL": "Scale/Speed",
            "NAME": "fx_scale_speed",
            "TYPE": "float",
            "MIN": 0.0,
            "MAX": 4.0,
            "DEFAULT": 1.0
        },
        {
            "LABEL": "Scale/BPM Sync",
            "NAME": "fx_scale_bpm_sync",
            "TYPE": "bool",
            "DEFAULT": false,
            "FLAGS": "button"
        },
        {
            "LABEL": "Scale/Reverse",
            "NAME": "fx_scale_reverse",
            "TYPE": "bool",
            "DEFAULT": 0,
            "FLAGS": "button"
        },
        {
            "Label": "Scale/Offset",
            "NAME": "fx_scale_offset",
            "TYPE": "float",
            "MIN": 0.0,
            "MAX": 1.0,
            "DEFAULT": 0.0
        },

        {
            "Label": "Scale/Strobe",
            "NAME": "fx_scale_strob",
            "TYPE": "float",
            "MIN": 0.0,
            "MAX": 1.0,
            "DEFAULT": 0.0
        },

        {
            "LABEL": "Scale/Restart",
            "NAME": "fx_scale_restart",
            "TYPE": "event",
        },

        {
            "LABEL": "Tile Size/Animate",
            "NAME": "fx_animate_tile_size",
            "TYPE": "bool",
            "DEFAULT": false,
            "FLAGS": "button"
        },

        {
            "LABEL": "Tile Size/Range",
            "NAME": "fx_tile_size_range",
            "TYPE": "floatRange",
            "DEFAULT": [0.0,1.0],
            "MIN": 0.0,
            "MAX": 1.0
        },
        {
            "LABEL": "Tile Size/Tile Offset",
            "NAME": "fx_tile_size_region_offset",
            "TYPE": "float",
            "MIN": 0.0,
            "MAX": 1.0,
            "DEFAULT": 0.25
        },

        {
            "LABEL": "Tile Size/Signal",
            "NAME": "fx_tile_size_signal",
            "TYPE": "long",
            "VALUES": ["Saw","Inverse Saw","Square","Inverse Square","Triangle","Sine"],
            "DEFAULT": "Saw"
        },
        {
            "LABEL": "Tile Size/Filter",
            "NAME": "fx_tile_size_filter",
            "TYPE": "long",
            "VALUES": ["Ease In","Ease Out","Ease In Out","Ease Out In"],
            "DEFAULT": "Ease In Out"
        },
        {
            "LABEL": "Tile Size/Curve",
            "NAME": "fx_tile_size_curve",
            "TYPE": "float",
            "MIN": 1.0,
            "MAX": 8.0,
            "DEFAULT": 1.0
        },

        {
            "LABEL": "Tile Size/Speed",
            "NAME": "fx_tile_size_speed",
            "TYPE": "float",
            "MIN": 0.0,
            "MAX": 4.0,
            "DEFAULT": 1.0
        },
        {
            "LABEL": "Tile Size/BPM Sync",
            "NAME": "fx_tile_size_bpm_sync",
            "TYPE": "bool",
            "DEFAULT": false,
            "FLAGS": "button"
        },
        {
            "LABEL": "Tile Size/Reverse",
            "NAME": "fx_tile_size_reverse",
            "TYPE": "bool",
            "DEFAULT": 0,
            "FLAGS": "button"
        },
        {
            "Label": "Tile Size/Offset",
            "NAME": "fx_tile_size_offset",
            "TYPE": "float",
            "MIN": 0.0,
            "MAX": 1.0,
            "DEFAULT": 0.0
        },
        {
            "Label": "Tile Size/Strobe",
            "NAME": "fx_tile_size_strob",
            "TYPE": "float",
            "MIN": 0.0,
            "MAX": 1.0,
            "DEFAULT": 0.0
        },
        {
            "LABEL": "Tile Size/Restart",
            "NAME": "fx_tile_size_restart",
            "TYPE": "event",
        },



        {
            "LABEL": "Tile Position/Animate",
            "NAME": "fx_animate_tile_pos",
            "TYPE": "bool",
            "DEFAULT": false,
            "FLAGS": "button"
        },



        {
            "LABEL": "Tile Position/Mode",
            "NAME": "fx_tile_pos_mode",
            "TYPE": "long",
            "VALUES": ["Circular","Noise 1", "Noise 2"],
            "DEFAULT": "Circular"
        },

        {
            "LABEL": "Tile Position/Range",
            "NAME": "fx_tile_pos_range",
            "TYPE": "float",
            "DEFAULT": 0.5,
            "MIN": 0.0,
            "MAX": 1.0
        },
        {
            "LABEL": "Tile Position/Tile Offset",
            "NAME": "fx_tile_pos_region_offset",
            "TYPE": "float",
            "MIN": 0.0,
            "MAX": 1.0,
            "DEFAULT": 0.25
        },
        {
            "LABEL": "Tile Position/Reverse Odd Regions",
            "NAME": "fx_tile_pos_opp_direction",
            "TYPE": "bool",
            "DEFAULT": false,
            "FLAGS": "button"
        },
        {
            "LABEL": "Tile Position/Speed",
            "NAME": "fx_tile_pos_speed",
            "TYPE": "float",
            "MIN": 0.0,
            "MAX": 4.0,
            "DEFAULT": 1.0
        },
        {
            "LABEL": "Tile Position/BPM Sync",
            "NAME": "fx_tile_pos_bpm_sync",
            "TYPE": "bool",
            "DEFAULT": false,
            "FLAGS": "button"
        },
        {
            "LABEL": "Tile Position/Reverse",
            "NAME": "fx_tile_pos_reverse",
            "TYPE": "bool",
            "DEFAULT": 0,
            "FLAGS": "button"
        },
        {
            "Label": "Tile Position/Offset",
            "NAME": "fx_tile_pos_offset",
            "TYPE": "float",
            "MIN": 0.0,
            "MAX": 1.0,
            "DEFAULT": 0.0
        },
        {
            "Label": "Tile Position/Offset Scale",
            "NAME": "fx_tile_pos_offset_scale",
            "TYPE": "float",
            "MIN": 0.0,
            "MAX": 4.0,
            "DEFAULT": 1.0
        },
        {
            "Label": "Tile Position/Strobe",
            "NAME": "fx_tile_pos_strob",
            "TYPE": "float",
            "MIN": 0.0,
            "MAX": 1.0,
            "DEFAULT": 0.0
        },
        {
            "LABEL": "Tile Position/Restart",
            "NAME": "fx_tile_pos_restart",
            "TYPE": "event",
        },

        {
            "LABEL": "Tile Opacity/Animate",
            "NAME": "fx_animate_tile_opacity",
            "TYPE": "bool",
            "DEFAULT": false,
            "FLAGS": "button"
        },

        {
            "LABEL": "Tile Opacity/Range",
            "NAME": "fx_tile_opacity_range",
            "TYPE": "floatRange",
            "DEFAULT": [0.0,1.0],
            "MIN": 0.0,
            "MAX": 1.0
        },
        {
            "LABEL": "Tile Opacity/Tile Offset",
            "NAME": "fx_tile_opacity_region_offset",
            "TYPE": "float",
            "MIN": 0.0,
            "MAX": 1.0,
            "DEFAULT": 0.25
        },

        {
            "LABEL": "Tile Opacity/Signal",
            "NAME": "fx_tile_opacity_signal",
            "TYPE": "long",
            "VALUES": ["Saw","Inverse Saw","Square","Inverse Square","Triangle","Sine"],
            "DEFAULT": "Sine"
        },
        {
            "LABEL": "Tile Opacity/Filter",
            "NAME": "fx_tile_opacity_filter",
            "TYPE": "long",
            "VALUES": ["Ease In","Ease Out","Ease In Out","Ease Out In"],
            "DEFAULT": "Ease In Out"
        },
        {
            "LABEL": "Tile Opacity/Curve",
            "NAME": "fx_tile_opacity_curve",
            "TYPE": "float",
            "MIN": 1.0,
            "MAX": 8.0,
            "DEFAULT": 1.0
        },

        {
            "LABEL": "Tile Opacity/Speed",
            "NAME": "fx_tile_opacity_speed",
            "TYPE": "float",
            "MIN": 0.0,
            "MAX": 4.0,
            "DEFAULT": 1.0
        },
        {
            "LABEL": "Tile Opacity/BPM Sync",
            "NAME": "fx_tile_opacity_bpm_sync",
            "TYPE": "bool",
            "DEFAULT": false,
            "FLAGS": "button"
        },
        {
            "LABEL": "Tile Opacity/Reverse",
            "NAME": "fx_tile_opacity_reverse",
            "TYPE": "bool",
            "DEFAULT": 0,
            "FLAGS": "button"
        },
        {
            "Label": "Tile Opacity/Offset",
            "NAME": "fx_tile_opacity_offset",
            "TYPE": "float",
            "MIN": 0.0,
            "MAX": 1.0,
            "DEFAULT": 0.0
        },
        {
            "Label": "Tile Opacity/Strobe",
            "NAME": "fx_tile_opacity_strob",
            "TYPE": "float",
            "MIN": 0.0,
            "MAX": 1.0,
            "DEFAULT": 0.0
        },
        {
            "LABEL": "Tile Opacity/Restart",
            "NAME": "fx_tile_opacity_restart",
            "TYPE": "event",
        },



        {
            "Label": "Region 1/Tile Size",
            "NAME": "fx_r1_tile_size",
            "TYPE": "float",
            "MIN": 0.01,
            "MAX": 1.0,
            "DEFAULT": 1.0
        },
        {
            "LABEL": "Region 1/Shift",
            "NAME": "fx_r1_offset",
            "TYPE": "point2D",
            "MIN": [-1.0,-1.0],
            "MAX": [1.0,1.0],
            "DEFAULT": [0.0,0.0]
        },
        {
            "Label": "Region 2/Tile Size",
            "NAME": "fx_r2_tile_size",
            "TYPE": "float",
            "MIN": 0.01,
            "MAX": 1.0,
            "DEFAULT": 1.0
        },
        {
            "LABEL": "Region 2/Shift",
            "NAME": "fx_r2_offset",
            "TYPE": "point2D",
            "MIN": [-1.0,-1.0],
            "MAX": [1.0,1.0],
            "DEFAULT": [0.0,0.0]
        },
        {
            "Label": "Region 3/Tile Size",
            "NAME": "fx_r3_tile_size",
            "TYPE": "float",
            "MIN": 0.01,
            "MAX": 1.0,
            "DEFAULT": 1.0
        },
        {
            "LABEL": "Region 3/Shift",
            "NAME": "fx_r3_offset",
            "TYPE": "point2D",
            "MIN": [-1.0,-1.0],
            "MAX": [1.0,1.0],
            "DEFAULT": [0.0,0.0]
        },
        {
            "Label": "Region 4/Tile Size",
            "NAME": "fx_r4_tile_size",
            "TYPE": "float",
            "MIN": 0.01,
            "MAX": 1.0,
            "DEFAULT": 1.0
        },
        {
            "LABEL": "Region 4/Shift",
            "NAME": "fx_r4_offset",
            "TYPE": "point2D",
            "MIN": [-1.0,-1.0],
            "MAX": [1.0,1.0],
            "DEFAULT": [0.0,0.0]
        },


         {
            "LABEL": "Tile Mask/Enable",
            "NAME": "fx_mask_enable",
            "TYPE": "bool",
            "DEFAULT": false,
            "FLAGS": "button"
        },
        {
            "LABEL": "Tile Mask/Opacity",
            "NAME": "fx_mask_opacity",
            "TYPE": "float",
            "MIN": 0.0,
            "MAX": 1.0,
            "DEFAULT": 1.0
        },
        {
            "LABEL": "Tile Mask/Texture",
            "NAME": "fx_tex_mask",
            "TYPE": "image"
        },
        {
            "LABEL": "Tile Mask/Scale",
            "NAME": "fx_mask_scale",
            "TYPE": "float",
            "MIN": 0.0,
            "MAX": 1.0,
            "DEFAULT": 1.0
        },
        {
            "LABEL": "Tile Mask/Aspect",
            "NAME": "fx_mask_aspect",
            "TYPE": "float",
            "MIN": 0.0,
            "MAX": 4.0,
            "DEFAULT": 1.0
        },
        {
            "Label": "Tile Mask/Rotate",
            "NAME": "fx_mask_rotate",
            "TYPE": "float",
            "MIN": -360.0,
            "MAX": 360.,
            "DEFAULT": 0.0
        },
        {
            "LABEL": "Tile Mask/Shift",
            "NAME": "fx_mask_offset",
            "TYPE": "point2D",
            "MIN": [-1.0,-1.0],
            "MAX": [1.0,1.0],
            "DEFAULT": [0.0,0.0]
        },
        {
            "LABEL": "Tile Mask/Flip X",
            "NAME": "fx_mask_flip_x",
            "TYPE": "bool",
            "DEFAULT": false,
            "FLAGS": "button"
        },
        {
            "LABEL": "Tile Mask/Flip Y",
            "NAME": "fx_mask_flip_y",
            "TYPE": "bool",
            "DEFAULT": false,
            "FLAGS": "button"
        },
        {
            "LABEL": "Tile Mask/Brightness",
            "NAME": "fx_mask_brightness",
            "TYPE": "float",
            "MIN": -1.0,
            "MAX": 1.0,
            "DEFAULT": 0.0
        },
        {
            "LABEL": "Tile Mask/Contrast",
            "NAME": "fx_mask_contrast",
            "TYPE": "float",
            "MIN": 0.0,
            "MAX": 4.0,
            "DEFAULT": 1
        },
        {
            "LABEL": "Tile Mask/Invert",
            "NAME": "fx_mask_invert",
            "TYPE": "bool",
            "DEFAULT": 0,
            "FLAGS": "button"
        },
        {
            "LABEL": "Tile Mask/Mode",
            "NAME": "fx_mask_mode",
            "TYPE": "long",
            "VALUES": ["Before Overlay","After Overlay"],
            "DEFAULT": "Before Overlay"
        },

        {
            "LABEL": "Tile Overlay/Enable",
            "NAME": "fx_overlay_enable",
            "TYPE": "bool",
            "DEFAULT": false,
            "FLAGS": "button"
        },
        {
            "LABEL": "Tile Overlay/Opacity",
            "NAME": "fx_overlay_opacity",
            "TYPE": "float",
            "MIN": 0.0,
            "MAX": 1.0,
            "DEFAULT": 1.0
        },
        {
            "LABEL": "Tile Overlay/Blend Mode",
            "NAME": "fx_overlay_mode",
            "TYPE": "long",
            "VALUES": ["Add","Over"],
            "DEFAULT": "Add"
        },
        {
            "LABEL": "Tile Overlay/Texture",
            "NAME": "fx_tex_overlay",
            "TYPE": "image"
        },
        {
            "LABEL": "Tile Overlay/Scale",
            "NAME": "fx_overlay_scale",
            "TYPE": "float",
            "MIN": 0.0,
            "MAX": 1.0,
            "DEFAULT": 1.0
        },
        {
            "LABEL": "Tile Overlay/Aspect",
            "NAME": "fx_overlay_aspect",
            "TYPE": "float",
            "MIN": 0.0,
            "MAX": 4.0,
            "DEFAULT": 1.0
        },
        {
            "Label": "Tile Overlay/Rotate",
            "NAME": "fx_overlay_rotate",
            "TYPE": "float",
            "MIN": -360.0,
            "MAX": 360.,
            "DEFAULT": 0.0
        },
        {
            "LABEL": "Tile Overlay/Shift",
            "NAME": "fx_overlay_offset",
            "TYPE": "point2D",
            "MIN": [-1.0,-1.0],
            "MAX": [1.0,1.0],
            "DEFAULT": [0.0,0.0]
        },
        {
            "LABEL": "Tile Overlay/Flip X",
            "NAME": "fx_overlay_flip_x",
            "TYPE": "bool",
            "DEFAULT": false,
            "FLAGS": "button"
        },
        {
            "LABEL": "Tile Overlay/Flip Y",
            "NAME": "fx_overlay_flip_y",
            "TYPE": "bool",
            "DEFAULT": false,
            "FLAGS": "button"
        },
        {
            "LABEL": "Tile Overlay/Brightness",
            "NAME": "fx_overlay_brightness",
            "TYPE": "float",
            "MIN": -1.0,
            "MAX": 1.0,
            "DEFAULT": 0.0
        },
        {
            "LABEL": "Tile Overlay/Contrast",
            "NAME": "fx_overlay_contrast",
            "TYPE": "float",
            "MIN": 0.0,
            "MAX": 4.0,
            "DEFAULT": 1
        },
        {
            "LABEL": "Tile Overlay/Saturation",
            "NAME": "fx_overlay_saturation",
            "TYPE": "float",
            "MIN": -1.0,
            "MAX": 3.0,
            "DEFAULT": 0.0
        },
        {
            "LABEL": "Tile Overlay/Hue",
            "NAME": "fx_overlay_hue_shift",
            "TYPE": "float",
            "MIN": 0.0,
            "MAX": 1,
            "DEFAULT": 0.0
        },
        {
            "LABEL": "Tile Overlay/Invert",
            "NAME": "fx_overlay_invert",
            "TYPE": "bool",
            "DEFAULT": 0,
            "FLAGS": "button"
        },

        {
            "LABEL": "Tile Borders/Enable",
            "NAME": "fx_tile_border_enable",
            "TYPE": "bool",
            "DEFAULT": false,
            "FLAGS": "button"
        },
        {
            "LABEL": "Tile Borders/Thickness",
            "NAME": "fx_tile_border_thick",
            "TYPE": "float",
            "MIN": 0.0,
            "MAX": 1.0,
            "DEFAULT": 0.25
        },
        {
            "LABEL": "Tile Borders/Shape",
            "NAME": "fx_tile_border_shape",
            "TYPE": "float",
            "MIN": 0.0,
            "MAX": 1.0,
            "DEFAULT": 1.0
        },
        {
            "LABEL": "Tile Borders/Curve",
            "NAME": "fx_tile_border_curve",
            "TYPE": "float",
            "MIN": 1.0,
            "MAX": 8.0,
            "DEFAULT": 1.0
        },
        {
            "LABEL": "Tile Borders/Alpha",
            "NAME": "fx_tile_border_alpha",
            "TYPE": "bool",
            "DEFAULT": false,
            "FLAGS": "button"
        },
        {
            "LABEL": "Tile Borders/Mode",
            "NAME": "fx_tile_border_mode",
            "TYPE": "long",
            "VALUES": ["Pre", "Post"],
            "DEFAULT": "Post",
            "FLAGS": "generate_as_define"
        },





        {
            "LABEL": "Color Controls/Color Controls",
            "NAME": "fx_color_controls_active",
            "TYPE": "bool",
            "DEFAULT": true,
            "FLAGS": "button"
        },
        {
            "LABEL": "Color Controls/Brightness",
            "NAME": "fx_brightness",
            "TYPE": "float",
            "MIN": -1.0,
            "MAX": 1.0,
            "DEFAULT": 0.0
        },
        {
            "LABEL": "Color Controls/Contrast",
            "NAME": "fx_contrast",
            "TYPE": "float",
            "MIN": 0.0,
            "MAX": 4.0,
            "DEFAULT": 1
        },
        {
            "LABEL": "Color Controls/Saturation",
            "NAME": "fx_saturation",
            "TYPE": "float",
            "MIN": -1.0,
            "MAX": 3.0,
            "DEFAULT": 0.0
        },
        {
            "LABEL": "Color Controls/Hue",
            "NAME": "fx_hue_shift",
            "TYPE": "float",
            "MIN": 0.0,
            "MAX": 1,
            "DEFAULT": 0.0
        },
        {
            "LABEL": "Color Controls/Invert",
            "NAME": "fx_invert",
            "TYPE": "bool",
            "DEFAULT": 0,
            "FLAGS": "button"
        },
        {
            "LABEL": "Alpha/Enable",
            "NAME": "fx_alpha",
            "TYPE": "bool",
            "DEFAULT": false,
            "FLAGS": "button"
        },


        {
            "LABEL": "Alpha/Luma to Alpha",
            "NAME": "fx_luma_to_alpha",
            "TYPE": "bool",
            "DEFAULT": false,
            "FLAGS": "button"
        },
        {
            "LABEL": "Alpha/Sensitivity",
            "NAME": "fx_luma_sensitivity",
            "TYPE": "float",
            "MIN": 0.01,
            "MAX": 4.0,
            "DEFAULT": 2.0
        },
        {
            "LABEL": "Alpha/Threshold",
            "NAME": "fx_luma_threshold",
            "TYPE": "float",
            "MIN": 0.0,
            "MAX": 1.0,
            "DEFAULT": 0.25
        },
        {
            "LABEL": "Alpha/Mode",
            "NAME": "fx_luma_mode",
            "TYPE": "long",
            "VALUES": ["Before Color Controls", "After Color Controls"],
            "DEFAULT": "After Color Controls",
            "FLAGS": "generate_as_define"
        },


    ],
    "GENERATORS": [
        {
            "NAME": "fx_shift_time_source",
            "TYPE": "time_base",
            "PARAMS": {
                "speed": "fx_shift_speed",
                "speed_curve":2,
                "reverse": "fx_shift_reverse",
                "strob" : "fx_shift_strob",
                "reset": "fx_shift_restart",
                "bpm_sync": "fx_shift_bpm_sync",
                "link_speed_to_global_bpm":true
            }
        },
        {
            "NAME": "fx_wobble_time_source",
            "TYPE": "time_base",
            "PARAMS": {
                "speed": "fx_wobble_speed",
                "speed_curve":2,
                "reverse": "fx_wobble_reverse",
                "strob" : "fx_wobble_strob",
                "reset": "fx_wobble_restart",
                "bpm_sync": "fx_wobble_bpm_sync",
                "link_speed_to_global_bpm":true
            }
        },
        {
            "NAME": "fx_rotate_time_source",
            "TYPE": "time_base",
            "PARAMS": {
                "speed": "fx_rotate_speed",
                "speed_curve":2,
                "strob" : "fx_rotate_strob",
                "reverse": "fx_rotate_reverse",
                "bpm_sync": "fx_rotate_bpm_sync",
                "reset": "fx_rotate_restart",
                "link_speed_to_global_bpm":true
            }
        },
        {
            "NAME": "fx_scale_time_source",
            "TYPE": "time_base",
            "PARAMS": {
                "speed": "fx_scale_speed",
                "speed_curve":2,
                "strob" : "fx_scale_strob",
                "reverse": "fx_scale_reverse",
                "bpm_sync": "fx_scale_bpm_sync",
                "reset": "fx_scale_restart",
                "link_speed_to_global_bpm":true
            }
        },
        {
            "NAME": "fx_tile_size_time_source",
            "TYPE": "time_base",
            "PARAMS": {
                "speed": "fx_tile_size_speed",
                "speed_curve":2,
                "reverse": "fx_tile_size_reverse",
                "strob" : "fx_tile_size_strob",
                "reset": "fx_tile_size_restart",
                "bpm_sync": "fx_tile_size_bpm_sync",
                "link_speed_to_global_bpm":true
            }
        },
        {
            "NAME": "fx_tile_pos_time_source",
            "TYPE": "time_base",
            "PARAMS": {
                "speed": "fx_tile_pos_speed",
                "speed_curve":2,
                "reverse": "fx_tile_pos_reverse",
                "strob" : "fx_tile_pos_strob",
                "reset": "fx_tile_pos_restart",
                "bpm_sync": "fx_tile_pos_bpm_sync",
                "link_speed_to_global_bpm":true
            }
        },
        {
            "NAME": "fx_tile_opacity_time_source",
            "TYPE": "time_base",
            "PARAMS": {
                "speed": "fx_tile_opacity_speed",
                "speed_curve":2,
                "reverse": "fx_tile_opacity_reverse",
                "strob" : "fx_tile_opacity_strob",
                "reset": "fx_tile_opacity_restart",
                "bpm_sync": "fx_tile_opacity_bpm_sync",
                "link_speed_to_global_bpm":true
            }
        }
    ],
    "IMPORTED": [
        {
            "NAME": "noiseLUT",
            "PATH": "noiseLUT.png",
            "GL_TEXTURE_MIN_FILTER": "LINEAR",
            "GL_TEXTURE_MAG_FILTER": "LINEAR",
            "GL_TEXTURE_WRAP": "REPEAT"
        }
    ]

}*/

/*
By Jason Beyers - January 2024
*/

#define NOISE_TEXTURE_BASED
#include "MadCommon.glsl"
#include "MadNoise.glsl"

// Timers
// Most of these are modified in other functions
float fx_shift_time = (fx_shift_time_source - fx_shift_offset * fx_shift_offset_scale * 4.) * 0.1;
float fx_wobble_time = (fx_wobble_time_source - fx_wobble_offset * fx_wobble_offset_scale * 4.);
float fx_rotate_time = fract((fx_rotate_time_source * 0.05  - fx_rotate_offset));
float fx_scale_time = fract(fx_scale_time_source * 0.125 - fx_scale_offset);
float fx_tile_size_time = (fx_tile_size_time_source - fx_tile_size_offset * 2.);
float fx_tile_pos_time = (fx_tile_pos_time_source - fx_tile_pos_offset * 2. * fx_tile_pos_offset_scale);
float fx_tile_opacity_time = (fx_tile_opacity_time_source - fx_tile_opacity_offset * 2.);


float fx_luma(vec3 color) {
  return dot(color, vec3(0.299, 0.587, 0.114));
}

float fx_luma(vec4 color) {
  return dot(color.rgb, vec3(0.299, 0.587, 0.114));
}

// Easing & filtering functions

float fxEaseInOut(float t, float curve) {
    // Simple ease-in-out function
    if (t < 0.5) {
        return pow(2 * t, curve) / 2.;
    } else {
        return 1. - pow(-2. * t + 2, curve) / 2.;
    }
}

float fxEaseIn(float t, float curve) {
    // Simple ease-in function
    return pow(t, curve);
}

float fxEaseOut(float t, float curve) {
    // Simple ease-out function
    return 1. - pow(1. - t, curve);
}

float fxEaseOutIn(float t, float curve) {
    // Ease-out-in function
    // This gets squirrely with high curve values
    return (pow(t, 3.) - 2. * pow(t, 2.) + t) * curve + (-2. * pow(t,3.) + 3. * pow(t, 2.)) + (pow(t, 3.) - pow(t, 2.)) * curve;
}


float fxFilter(float t, long filter_type, float curve) {
    // Apply one of four filters to time-varying variable t (ranging from 0 to 1) with curve

    if (filter_type == 0) { // Ease In
        return fxEaseIn(t, curve);
    } else if (filter_type == 1) { // Ease Out
        return fxEaseOut(t, curve);
    } else if (filter_type == 2) { // Ease In Out
        return fxEaseInOut(t, curve);
    } else { // Ease Out In
        return fxEaseOutIn(t, curve);
    }
}

// Various helper functions

float fxCircle(in vec2 _st, in float _radius){
    vec2 dist = _st-vec2(0.5);
    return 1.-smoothstep(_radius-(_radius*0.01),
                         _radius+(_radius*0.01),
                         dot(dist,dist)*4.0);
}

float getNoise(vec2 p) {
    p += vec2(0.5);
    p *= 0.1;
    p -= vec2(0.5);
    float value = texture(noiseLUT, p).x * 0.5; // Adjust the texture sampler and scale as needed
    // return clamp(value, 0.2, 0.8);
    return value;
}


vec2 uvNoiseOffset(vec2 uv, float time) {
    vec2 noiseOffset = vec2(getNoise(uv + time), getNoise(uv - time));
    return noiseOffset;
}

// UV transform functions

vec2 fxRot2D(vec2 _st, float _angle){
    _st -= 0.5;
    _st =  mat2(cos(_angle),-sin(_angle),
                sin(_angle),cos(_angle)) * _st;
    _st += 0.5;
    return _st;
}


vec2 applyScale(vec2 uv) {
    // Apply UV scale transforms to main output

    if (fx_animate_scale) {
        // scale_time = fract(fx_scale_time);

        float scale_time;

        if (fx_scale_signal == 0) { // Saw
            scale_time = fx_scale_time;
        } else if (fx_scale_signal == 1) { // Inverse Saw
            scale_time = 1. - fx_scale_time;
        } else if (fx_scale_signal == 2) { // Square
            scale_time = floor(fx_scale_time + 0.5);
        } else if (fx_scale_signal == 3) { // Inverse Square
            scale_time = 1. - floor(fx_scale_time + 0.5);
        } else if (fx_scale_signal == 4) { // Triangle
            scale_time = abs(0.5 - fx_scale_time);
        } else { // Sine
            scale_time = 0.5 + 0.5 * sin(2. * PI * fx_scale_time);
        }
        scale_time = fxFilter(scale_time, fx_scale_filter, fx_scale_curve);

        scale_time = 1. - scale_time;

        float range_min = fx_scale_range[0];
        float range_max = fx_scale_range[1];
        scale_time = range_min + scale_time * (range_max - range_min);

        if (fx_scale_mode == 0) { // Add
            return uv * fx_scale * (1. + scale_time);
        } else { // Subtract
            return uv * fx_scale / (1. + scale_time);
        }

    } else {
        return uv * fx_scale;
    }
}

vec2 applyUVShift(vec2 uv, float rotate) {
    // Apply UV shift for main output

    vec2 uv_shift = fx_shift_amount * fx_shift_scale * 0.5;
    uv_shift += vec2(0.5);
    uv_shift.x = 1.-uv_shift.x;
    uv_shift -= vec2(0.5);

    if (fx_shift_animate) {
        float shift_time_x = fx_shift_time * cos(2.*PI * fx_shift_angle/360.0);
        float shift_time_y = fx_shift_time * sin(2.*PI * fx_shift_angle/360.0);
        uv_shift.x -= shift_time_x;
        uv_shift.y -= shift_time_y;
    }
    uv_shift += vec2(0.5);
    uv_shift = fxRot2D(uv_shift, 2*PI*(rotate) / 360);
    uv_shift -= vec2(0.5);
    uv += uv_shift;
    return uv;
}

vec2 applyUVWobble(vec2 uv) {
    // Apply UV wobble for main output

    float tile_pos_time;

    if (fx_wobble_animate) {
        float wobble_time = fx_wobble_time;
        vec2 wobble;

        float range_min = 0.;
        float range_max = fx_wobble_range;

        if (fx_wobble_mode == 0) { // Circular

           wobble_time *= 4.;
            float power = 0.5;
            float range = range_min + 1. * (range_max - range_min);
            wobble = 0.25 * power * range * vec2(sin(wobble_time), cos(wobble_time));

        } else if (fx_wobble_mode == 1) { // Noise 1
            wobble_time /= 32.;
            wobble = uvNoiseOffset(vec2(0.5), wobble_time);
            wobble -= vec2(0.5);
            wobble *= 2.;
            wobble.x = range_min + (wobble.x + 0.5) * (range_max - range_min);
            wobble.y = range_min + (wobble.y + 0.5) * (range_max - range_min);
        } else {
            wobble_time /= 1.5;
            range_max /= 32.;
            wobble = dnoise(vec2(wobble_time, 0.)).yz;
            wobble -= vec2(0.5);
            wobble *= 2.;
            wobble.x = range_min + (wobble.x + 0.5) * (range_max - range_min);
            wobble.y = range_min + (wobble.y + 0.5) * (range_max - range_min);
        }
        return uv + wobble;
    } else {
        return uv;
    }
}

float getRotateValue() {
    // Return the rotation value (0-1) for main output

    float rotate;
    float rotate_time;

    if (fx_animate_rotate) {

        if (fx_rotate_signal == 0) { // Saw
            rotate_time = fx_rotate_time;
        } else if (fx_rotate_signal == 1) { // Inverse Saw
            rotate_time = 1. - fx_rotate_time;
        } else if (fx_rotate_signal == 2) { // Square
            rotate_time = floor(fx_rotate_time + 0.5);
        } else if (fx_rotate_signal == 3) { // Inverse Square
            rotate_time = 1. - floor(fx_rotate_time + 0.5);
        } else if (fx_rotate_signal == 4) { // Triangle
            rotate_time = abs(0.5 - fx_rotate_time);
        } else { // Sine
            rotate_time = 0.5 + 0.5 * sin(2. * PI * fx_rotate_time);
        }

        rotate_time = 1. - rotate_time;
        // rotate_time = matEaseInOut(rotate_time, fx_rotate_curve);
        rotate_time = fxFilter(rotate_time, fx_rotate_filter, fx_rotate_curve);
        float range_min = fx_rotate_range[0] / 360.0;
        float range_max = fx_rotate_range[1] / 360.0;
        rotate_time = range_min + rotate_time * (range_max - range_min);

        rotate = (rotate_time + fx_rotate / 360.) * 360.;
    } else {
        rotate = fx_rotate;
    }
    return rotate;
}

vec2 transformUV(vec2 uv) {
    // UV transforms for the main output

    // if (fx_lock_aspect) {
    //     uv.x *= RENDERSIZE.x / RENDERSIZE.y;
    // }
    // uv = applyScale(uv) * 0.5;

    uv -= vec2(0.5);
    uv = applyScale(uv);
    uv.x *= fx_aspect;
    uv += vec2(0.5);

    float rotate = getRotateValue();

    // XY shift pre rotate
    if (fx_shift_type == 0) {
        uv = applyUVShift(uv, -1 * rotate);
        uv = applyUVWobble(uv);
    }

    // uv += vec2(0.5);
    uv = fxRot2D(uv, 2*PI*rotate / 360);
    // uv -= vec2(0.5);

    // XY shift post rotate
    if (fx_shift_type == 1) {

        // Preserve direction
        uv = applyUVShift(uv, rotate);
        uv = applyUVWobble(uv);
    }
    return uv;
}

vec2 fx_hat(vec2 uv) {
    // Split the UV space into tiles
    float repeats = fx_repeats / 2.;
    uv = mod(uv * repeats / 2.0, 0.5);

    return 1.0 - 2. * abs(uv - 0.5);
}

vec2 applyGenericUV(vec2 uv, float scale, float aspect, float rotate, vec2 offset, bool flip_x, bool flip_y) {
    // Manipulate UV coordinates in a general way

    uv -= vec2(0.5);
    uv *= scale;
    uv.x *= aspect;
    uv += vec2(0.5);
    if (flip_x) {
        uv.x = 1. - uv.x;
    }
    if (flip_y) {
        uv.y = 1. - uv.y;
    }
    vec2 uv_shift = offset;
    uv_shift += vec2(0.5);
    uv_shift.x = 1.-uv_shift.x;
    uv_shift -= vec2(0.5);
    uv -= vec2(0.5);
    uv.x /= aspect;
    uv += vec2(0.5);
    uv = fxRot2D(uv, 2*PI*rotate / 360);
    uv -= vec2(0.5);
    uv.x *= aspect;
    uv += vec2(0.5);
    uv_shift += vec2(0.5);
    uv_shift = fxRot2D(uv_shift, 2*PI*(rotate) / 360);
    uv_shift -= vec2(0.5);
    uv += uv_shift;
    return uv;
}

vec4 applyColorControls(vec4 color, float brightness, float contrast, float saturation, float hue, bool invert) {
    // Apply color controls FX to the provided vec4 color

    // Apply invert
    if (invert) color.rgb=1-color.rgb;

    // Apply Hue Shift and saturation
    if (hue > 0.01 || saturation != 0) {
        vec3 hsv = rgb2hsv(color.rgb);
        hsv.x = fract(0.9999999*(hsv.x+hue));
        hsv.y = max(hsv.y + saturation, 0);
        color.rgb = hsv2rgb(hsv);
    }

    // Apply contrast
    const vec3 LumCoeff = vec3(0.2125, 0.7154, 0.0721);
    const vec3 AvgLumin = vec3(0.5, 0.5, 0.5);
    vec3 intensity = vec3(dot(color.rgb, LumCoeff));
    color.rgb = mix(AvgLumin, color.rgb, contrast);

    // Apply brightness
    color.rgb += brightness;

    return color;
}

vec4 applySoftBW(vec4 color, float fx_mix, float gain) {
    // Apply soft black & white FX to the provided vec4 color
    // This FX can do things that basic saturation & contrast cannot

    if (fx_mix > 0.0) {
        vec4 original = color;
        vec3 raw_color = color.rgb;
        float col_mag = (dot(vec3(1.0), raw_color.rgb) / 3.0 * gain);
        col_mag = smoothstep(0.0, 1.0, col_mag);
        col_mag = smoothstep(0.0, 1.0, col_mag);
        raw_color = vec3(1.0) * col_mag;
        color.rgb = raw_color;
        color.rgb = mix(color.rgb, original.rgb,1.-fx_mix);
    }
    return color;
}

float distToLine(vec2 pt1, vec2 pt2, vec2 testPt) {
    // Helper function for applySoftBorder

    vec2 lineDir = pt2 - pt1;
    vec2 perpDir = vec2(lineDir.y, -lineDir.x);
    vec2 dirToPt1 = pt1 - testPt;
    return abs(dot(normalize(perpDir), dirToPt1));
}

vec4 applySoftBorder(vec4 color, vec2 uv, long tile_shape, float fx_mix, float thick, float shape, float curve) {
    // Apply soft border FX to the provided vec4 color

    if (fx_mix > 0.0) {
        vec4 original = color;
        // uv += vec2(0.5); // ?
        float aspectRatio = 0.5;
        float alphaMult;

        if (tile_shape == 0) { // Square
            vec2 distFromBorder = min(uv,vec2(1)-uv);
            float borderWidthX, borderWidthY;
            if (aspectRatio > 0.5) {
                borderWidthX = thick;
                borderWidthY = thick * (1 - 2*(aspectRatio-0.5));
            } else {
                borderWidthY = thick;
                borderWidthX = thick * (1 - 2*(0.5-aspectRatio));
            }
            float distX=smoothstep(0,borderWidthX*borderWidthX,distFromBorder.x);
            float distY=smoothstep(0,borderWidthY*borderWidthY,distFromBorder.y);
            float dist=mix(min(distX,distY),sqrt(distX*distY),shape);
            alphaMult=dist;
        } else if (tile_shape == 1) { // Circle
            float distFromBorder = 0.5 - length(uv - vec2(0.5,0.5));
            alphaMult = distFromBorder / (thick*thick/2);
        } else { // Triangle
            // Apply transparency on borders
            float distFromBorder = min(distToLine(vec2(0,0),vec2(0.5,1),uv),min(distToLine(vec2(0.5,1),vec2(1,0),uv),distToLine(vec2(1,0),vec2(0,0),uv)));
            alphaMult = distFromBorder / (thick*thick/2);
        }
        if (fx_tile_border_alpha) {
            color.a *= pow(clamp(alphaMult,0,1),curve*curve);
        } else {
            color.rgb *= pow(clamp(alphaMult,0,1),curve*curve);
            color.a = 1.;
        }

        color.rgb = mix(color.rgb, original.rgb,1.-fx_mix);
    }
    return color;
}

// Texture sampling functions

vec2 mirrorUV(vec2 uv, bool mirror_x, bool mirror_y) {
    // UV is expected in 0-1 range
    if (mirror_x) {
        if (uv.x > 0.5) {
            uv.x = 1.0 - uv.x;
        }
    }
    if (mirror_y) {
        if (uv.y > 0.5) {
            uv.y = 1.0 - uv.y;
        }
    }
    return uv;
}

vec4 getInColor(vec2 uv) {
    uv = applyGenericUV(uv, fx_input_scale, fx_input_aspect, fx_input_rotate, fx_input_offset, fx_input_flip_x, fx_input_flip_y);
    uv = mirrorUV(uv, fx_input_mirror_x, fx_input_mirror_y);
    vec4 color = FX_NORM_PIXEL(uv);
    return color;
}

vec4 getOverlayColor(vec2 uv) {
    // Sample a custom overlay texture

    uv = applyGenericUV(uv, fx_overlay_scale, fx_overlay_aspect, fx_overlay_rotate,  fx_overlay_offset, fx_overlay_flip_x, fx_overlay_flip_y);
    vec4 color = IMG_NORM_PIXEL(fx_tex_overlay, uv);
    color = applyColorControls(color, fx_overlay_brightness, fx_overlay_contrast, fx_overlay_saturation, fx_overlay_hue_shift, fx_overlay_invert);
    return color;
}

vec4 getMaskColor(vec2 uv) {
    // Sample a custom mask texture

    uv = applyGenericUV(uv, fx_mask_scale, fx_mask_aspect, fx_mask_rotate, fx_mask_offset, fx_mask_flip_x, fx_mask_flip_y);
    vec4 color = IMG_NORM_PIXEL(fx_tex_mask, uv);
    color = applyColorControls(color, fx_mask_brightness, fx_mask_contrast, 0., 0., fx_mask_invert);
    return color;
}

// Functions that manipulate tiles (UV and/or color)

vec4 applyMask(vec4 color, vec2 uv) {
    // Multiply color by mask (luma only, not full color of mask)

    if (fx_mask_enable) {
        vec4 orig_color = color;
        color *= fx_luma(getMaskColor(uv).rgb);
        color = mix(orig_color, color, fx_mask_opacity);
    }
    return color;
}

vec4 overBlend(vec4 color1, vec4 color2) {
    // Implementation of the "Over" blend mode, from ChatGPT

    float alpha1 = color1.a;
    float alpha2 = color2.a;

    vec3 blendedRGB = (color1.rgb * alpha1 + color2.rgb * (1.0 - alpha1 * alpha2)) / (alpha1 + (1.0 - alpha1 * alpha2));
    float blendedAlpha = alpha1 + (1.0 - alpha1 * alpha2);

    return vec4(blendedRGB, blendedAlpha);
}

vec4 applyOverlay(vec4 color, vec2 uv) {
    // Add overlay color to the provided color input

    if (fx_overlay_enable) {
        vec4 overlay_color = getOverlayColor(uv) * fx_overlay_opacity;
        if (fx_overlay_mode == 0) { // Add
            color += overlay_color;
        } else { // Over (only useful when overlay has alpha channel)
            color = overBlend(overlay_color, color);
        }
    }
    return color;
}

vec4 applyTileFX(vec4 color, vec2 uv) {
    // Modify a color input and apply global tile FX

    if (fx_tile_border_enable) {
        color = applySoftBorder(color, uv, fx_tile_shape, 1., fx_tile_border_thick, fx_tile_border_shape, fx_tile_border_curve);
    }
    return color;
}

vec4 applyTileMask(vec4 color, vec2 uv) {
    // Mask the edges if the tile size is < 1

    // Global constraint
    float limit = (1. - fx_tile_size)/2.;

    if (fx_tile_shape == 0) { // Square mask
        if (uv.x < limit || uv.y < limit || (1.-uv.x) < limit || (1.-uv.y) < limit) {
            color = fx_tile_back_color;
        }
    } else { // Circle mask
        if (abs(length(vec2(0.5) - uv)) > (1. - 2.*limit) / 2.) {
            color = fx_tile_back_color;
        }
    }
    return color;
}

vec2 applyTileUV(vec2 uv) {
    // Constrain the UV if tile size is < 1 and return the modified UV

    // Global multiplier
    float multiplier = fx_tile_size;

    uv -= vec2(0.5);
    uv /= multiplier;
    uv += vec2(0.5);
    return uv;
}

float regionTileSize(int region) {
    // Return the size of a given region (1 being full size)
    // If tile size is not animated, return 1

    if (fx_animate_tile_size) {
        float mult = fract(region * fx_tile_size_region_offset - 0.25 + fx_tile_size_time);
        if (fx_tile_size_signal == 0) { // Saw
            mult = mult; // do nothing
        } else if (fx_tile_size_signal == 1) { // Inverse Saw
            mult = 1. - mult;
        } else if (fx_tile_size_signal == 2) { // Square
            mult = floor(mult + 0.5);
        } else if (fx_tile_size_signal == 3) { // Inverse Square
            mult = 1. - floor(mult + 0.5);
        } else if (fx_tile_size_signal == 4) { // Triangle
            mult = fract(region * fx_tile_size_region_offset - 0.25 + fx_tile_size_time / 2.);
            mult = 2. * abs(0.5 - mult);
        } else { // Sine
            mult = 0.5 + 0.5 * (sin(PI*(region * fx_tile_size_region_offset - 0.25 + fx_tile_size_time)));
        }
        mult = fxFilter(mult, fx_tile_size_filter, fx_tile_size_curve);
        float range_min = fx_tile_size_range[0];
        float range_max = fx_tile_size_range[1];
        mult = range_min + mult * (range_max - range_min);
        return mult;
    } else {
        return 1.;
    }
}

float regionTileOpacity(int region) {
    // Return a float representing the opacity of a given region
    // If opacity is not animated, always return 1

    if (fx_animate_tile_opacity) {
        float mult = fract(region * fx_tile_opacity_region_offset - 0.25 + fx_tile_opacity_time);
        if (fx_tile_opacity_signal == 0) { // Saw
            mult = mult; // do nothing
        } else if (fx_tile_opacity_signal == 1) { // Inverse Saw
            mult = 1. - mult;
        } else if (fx_tile_opacity_signal == 2) { // Square
            mult = floor(mult + 0.5);
        } else if (fx_tile_opacity_signal == 3) { // Inverse Square
            mult = 1. - floor(mult + 0.5);
        } else if (fx_tile_opacity_signal == 4) { // Triangle
            mult = fract(region * fx_tile_opacity_region_offset - 0.25 + fx_tile_opacity_time / 2.);
            mult = 2. * abs(0.5 - mult);
        } else { // Sine
            mult = 0.5 + 0.5 * (sin(PI*(region * fx_tile_opacity_region_offset - 0.25 + fx_tile_opacity_time)));
        }
        mult = fxFilter(mult, fx_tile_opacity_filter, fx_tile_opacity_curve);
        float range_min = fx_tile_opacity_range[0];
        float range_max = fx_tile_opacity_range[1];
        mult = range_min + mult * (range_max - range_min);
        return mult;
    } else {
        return 1.;
    }
}

vec2 regionTilePosition(int region) {
    // Return a vec2 representing an XY offset for tile position, introduced by animation
    // Range of output: [-1,-1] to [1,1]
    // Return vec2(0.) if tile pos animation is disabled

    vec2 pos = vec2(0.);

    if (fx_animate_tile_pos) {

        float tile_pos_time = region * fx_tile_pos_region_offset - 0.25 + fx_tile_pos_time;
        float range_min = 0.;
        float range_max = fx_tile_pos_range;

        if (fx_tile_pos_opp_direction) {
            if (region == 1 || region == 2) {
                tile_pos_time = -1 * tile_pos_time;
            }
        }

        if (fx_tile_pos_mode == 0) { // Circular
            tile_pos_time *= 4.;
            float power = 0.5;
            float range = range_min + 1. * (range_max - range_min);
            pos = 0.25 * power * range * vec2(sin(tile_pos_time), cos(tile_pos_time));

        } else if (fx_tile_pos_mode == 1) { // Noise 1
            tile_pos_time /= 32.;
            pos = uvNoiseOffset(vec2(0.5), tile_pos_time);
            pos -= vec2(0.5);
            pos *= 2.;
            pos.x = range_min + (pos.x + 0.5) * (range_max - range_min);
            pos.y = range_min + (pos.y + 0.5) * (range_max - range_min);
        } else {
            tile_pos_time /= 1.5;
            range_max /= 32.;
            pos = dnoise(vec2(tile_pos_time, region * fx_tile_pos_region_offset)).yz;
            pos -= vec2(0.5);
            pos *= 2.;
            pos.x = range_min + (pos.x + 0.5) * (range_max - range_min);
            pos.y = range_min + (pos.y + 0.5) * (range_max - range_min);
        }
    }
    return pos;
}

vec2 applyRegionUV(vec2 uv, int region) {
    // Apply XY shift and scale modifications to a region's UV coordinates
    // Return a modified vec2 representing new coordinates

    vec2 shift = vec2(0.);
    float multiplier = 1.;
    if (region == 1) {
        shift = fx_r1_offset * 0.5;
        shift += vec2(0.5);
        shift.x = 1.-shift.x;
        shift -= vec2(0.5);
        multiplier = fx_r1_tile_size;
    } else if (region == 2) {
        shift = fx_r2_offset * 0.5;
        shift += vec2(0.5);
        shift.x = 1.-shift.x;
        shift -= vec2(0.5);
        multiplier = fx_r2_tile_size;
    } else if (region == 3) {
        shift = fx_r3_offset * 0.5;
        shift += vec2(0.5);
        shift.x = 1.-shift.x;
        shift -= vec2(0.5);
        multiplier = fx_r3_tile_size;
    } else if (region == 4) {
        shift = fx_r4_offset * 0.5;
        shift += vec2(0.5);
        shift.x = 1.-shift.x;
        shift -= vec2(0.5);
        multiplier = fx_r4_tile_size;
    }

    // Animate the shift, if applicable
    shift += regionTilePosition(region);

    if (fx_constrain_tiles) {
        multiplier /= 2. * length(vec2(0.0) - shift) + 1.;
    }
    multiplier *= regionTileSize(region);
    uv -= vec2(0.5);

    uv /= multiplier;
    uv += vec2(0.5);
    uv += shift;
    return uv;
}

vec4 sampleTexture(vec2 uv, int region) {
    // Sample input texture with overlay & mask

    vec4 color;

    if (region == 1) {
        uv = uv * 2.;
    } else if (region == 2) {
        uv = (uv - vec2(0.,0.5)) * 2.;
    } else if (region == 3) {
        uv = (uv - vec2(0.5,0.)) * 2.;
    } else {
        uv = (uv - vec2(0.5)) * 2.;
    }

    // fx_tile_mode==0 --> basic

    if (fx_tile_mode == 1) { // Flip X by column
        if (region == 1 || region == 2) {
            uv.x = 1. - uv.x;
        }

    } else if (fx_tile_mode == 2) { // Flip Y by column
        if (region == 1 || region == 2) {
            uv.y = 1. - uv.y;
        }

    } else if (fx_tile_mode == 3) { // Flip X by row
        if (region == 2 || region == 4) {
            uv.x = 1. - uv.x;
        }

    } else if (fx_tile_mode == 4) { // Flip Y by row
        if (region == 1 || region == 3) {
            uv.y = 1. - uv.y;
        }

    } else if (fx_tile_mode == 5) { // Flip X corner
        if (region == 1 || region == 4) {
            uv.x = 1. - uv.x;
        }

    } else if (fx_tile_mode == 6) { // Flip Y corner
        if (region == 1 || region == 4) {
            uv.y = 1. - uv.y;
        }
    }

    // Manipulate region and tile UV, to accommodate various user inputs
    uv = applyRegionUV(uv, region);
    vec2 uv_orig = uv;
    uv = applyTileUV(uv);

    color = getInColor(uv);

    if (fx_tile_border_mode == 0) { // Tile FX before mask & overlay
        color = applyTileFX(color, uv);
    }

    if (fx_mask_mode == 0) { // Mask before overlay
        color = applyMask(color, uv);
        color = applyOverlay(color, uv);
    } else { // Mask after overlay
        color = applyOverlay(color, uv);
        color = applyMask(color, uv);
    }

    vec4 masked_color = applyTileMask(color, uv_orig);

    color = masked_color;

    // Modify the opacity, if opacity is being animated
    if (masked_color != fx_tile_back_color) { // Ugly hack to check if a tile mask has obscured something, and only apply opacity animation to the foreground
        color.rgb *= regionTileOpacity(region);
    }

    if (fx_tile_border_mode == 1) { // Tile FX after mask & overlay
        color = applyTileFX(color, uv);
    }

    return color;
}

vec4 fxColorForPixel(vec2 mm_FragNormCoord)
{
    vec2 uv = mm_FragNormCoord;
    vec4 fx_out; // FX output color before mix + post
    vec4 out_color; // Final output color after post

    // FX code below
    vec4 fx_orig = FX_NORM_PIXEL(uv);

    vec2 uv_orig = uv;

    // Global UV transforms
    uv = transformUV(uv);

    uv = fx_hat(uv); // First round of tiling happens here
    // At this point, 1 tile = 4 regions
    // From this point on, "tile" refers to a quarter of that

    int region; // Region of screen (1-4)

    if (uv.x < 0.5 && uv.y < 0.5) {
        region = 1;
    } else if (uv.x < 0.5 && uv.y > 0.5) {
        region = 2;
    } else if (uv.x > 0.5 && uv.y < 0.5) {
        region = 3;
    } else {
        region = 4;
    }

    fx_out = sampleTexture(uv, region);

    // Luma to alpha (before color controls)
    if (fx_luma_to_alpha && fx_luma_mode == 0) {
        out_color.a = fx_luma(out_color * fx_luma_sensitivity) - fx_luma_threshold * 4. - 1.;
    }

    // Mix before color controls
    out_color = mix(fx_orig, fx_out, fx_filter_mix);

    if (fx_mix_add_mode==0) {
        out_color += fx_mix_add * fx_orig;
    } else {
        out_color = max(out_color, fx_mix_add * fx_orig);
    }

    if (fx_color_controls_active)
    {
        // Apply invert
        if (fx_invert) out_color.rgb=1-out_color.rgb;

        // Apply Hue Shift and saturation
        if (fx_hue_shift > 0.01 || fx_saturation != 0) {
            vec3 hsv = rgb2hsv(out_color.rgb);
            hsv.x = fract(0.9999999*(hsv.x+fx_hue_shift));
            hsv.y = max(hsv.y + fx_saturation, 0);
            out_color.rgb = hsv2rgb(hsv);
        }

        // Apply contrast
        const vec3 LumCoeff = vec3(0.2125, 0.7154, 0.0721);
        const vec3 AvgLumin = vec3(0.5, 0.5, 0.5);
        vec3 intensity = vec3(dot(out_color.rgb, LumCoeff));
        out_color.rgb = mix(AvgLumin, out_color.rgb, fx_contrast);

        // Apply brightness
        out_color.rgb += fx_brightness;
    }

    // Luma to alpha (after color controls)
    if (fx_luma_to_alpha && fx_luma_mode == 1) {
        out_color.a = fx_luma(out_color * fx_luma_sensitivity);
    }

    if (!fx_alpha) {
        out_color.a = 1.;
    }

    return out_color;
}
